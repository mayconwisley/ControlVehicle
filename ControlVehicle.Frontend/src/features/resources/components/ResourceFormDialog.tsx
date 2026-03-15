import { useEffect, useMemo, useState } from "react";
import {
  Autocomplete,
  Button,
  Checkbox,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControlLabel,
  MenuItem,
  Stack,
  TextField
} from "@mui/material";
import { getResourcePage } from "../../../shared/api/client";
import type { ResourceDefinition, ResourceFormField } from "../config/resources";

type LookupOption = {
  id: string;
  label: string;
};

type ResourceFormDialogProps = {
  open: boolean;
  isEdit: boolean;
  loading: boolean;
  definition: ResourceDefinition;
  initialData?: Record<string, unknown> | null;
  onClose: () => void;
  onSubmit: (payload: Record<string, unknown>) => Promise<void>;
};

const formatDateTimeLocal = (value: unknown) => {
  if (typeof value !== "string" || value.length === 0) {
    return "";
  }

  const date = new Date(value);
  if (Number.isNaN(date.getTime())) {
    return "";
  }

  const tzOffset = date.getTimezoneOffset() * 60000;
  const local = new Date(date.getTime() - tzOffset);
  return local.toISOString().slice(0, 16);
};

const toFieldValue = (field: ResourceFormField, value: unknown) => {
  if (value === undefined || value === null) {
    return field.type === "checkbox" ? false : "";
  }

  if (field.type === "date") {
    return String(value).slice(0, 10);
  }

  if (field.type === "datetime") {
    return formatDateTimeLocal(value);
  }

  if (field.type === "checkbox") {
    return Boolean(value);
  }

  return String(value);
};

const parseValue = (field: ResourceFormField, value: unknown) => {
  if (field.type === "checkbox") {
    return Boolean(value);
  }

  if (field.type === "number") {
    const parsed = Number(value);
    return Number.isNaN(parsed) ? 0 : parsed;
  }

  if (
    field.type === "text" ||
    field.type === "select" ||
    field.type === "date" ||
    field.type === "datetime" ||
    field.type === "lookup"
  ) {
    const text = String(value ?? "").trim();
    return text.length === 0 && !field.required ? null : text;
  }

  return value;
};

const toLookupLabel = (endpoint: "Driver" | "Vehicle", item: Record<string, unknown>) => {
  if (endpoint === "Driver") {
    const name = String(item.name ?? "").trim();
    const cnh = String(item.cnh ?? "").trim();
    if (name && cnh) {
      return `${name} - CNH ${cnh}`;
    }

    return name || cnh || String(item.id ?? "");
  }

  const model = String(item.model ?? "").trim();
  const plate = String(item.licensePlate ?? "").trim();
  if (model && plate) {
    return `${model} - ${plate}`;
  }

  return model || plate || String(item.id ?? "");
};

const toLookupOptions = (endpoint: "Driver" | "Vehicle", items: Record<string, unknown>[]) =>
  items
    .map((item) => {
      const id = String(item.id ?? "").trim();
      if (!id) {
        return null;
      }

      return {
        id,
        label: toLookupLabel(endpoint, item)
      } satisfies LookupOption;
    })
    .filter((option): option is LookupOption => option !== null);

export const ResourceFormDialog = ({
  open,
  isEdit,
  loading,
  definition,
  initialData,
  onClose,
  onSubmit
}: ResourceFormDialogProps) => {
  const lookupFields = useMemo(
    () => definition.formFields.filter((field) => field.type === "lookup" && field.lookup),
    [definition.formFields]
  );

  const initialValues = useMemo(() => {
    const values: Record<string, unknown> = {};
    definition.formFields.forEach((field) => {
      values[field.name] = toFieldValue(field, initialData?.[field.name]);
    });
    return values;
  }, [definition, initialData]);

  const [formValues, setFormValues] = useState<Record<string, unknown>>(initialValues);
  const [lookupInput, setLookupInput] = useState<Record<string, string>>({});
  const [lookupOptions, setLookupOptions] = useState<Record<string, LookupOption[]>>({});
  const [lookupLoading, setLookupLoading] = useState<Record<string, boolean>>({});

  useEffect(() => {
    setFormValues(initialValues);

    const nextLookupInput: Record<string, string> = {};
    lookupFields.forEach((field) => {
      nextLookupInput[field.name] = "";
    });

    setLookupInput(nextLookupInput);
  }, [initialValues, lookupFields]);

  useEffect(() => {
    if (!open || lookupFields.length === 0) {
      return;
    }

    const timer = setTimeout(() => {
      lookupFields.forEach((field) => {
        const endpoint = field.lookup?.endpoint;
        if (!endpoint) {
          return;
        }

        const search = lookupInput[field.name] ?? "";

        setLookupLoading((previous) => ({ ...previous, [field.name]: true }));

        void getResourcePage<Record<string, unknown>>(endpoint, 1, 10, search)
          .then(async (response) => {
            const options = toLookupOptions(endpoint, response.items);
            const selectedId = String(formValues[field.name] ?? "").trim();
            const selectedExists = options.some((option) => option.id === selectedId);

            if (selectedId && !selectedExists) {
              const byIdResponse = await getResourcePage<Record<string, unknown>>(endpoint, 1, 1, selectedId);
              const byIdOptions = toLookupOptions(endpoint, byIdResponse.items);
              const selectedOption = byIdOptions.find((option) => option.id === selectedId);

              if (selectedOption) {
                options.unshift(selectedOption);
              }
            }

            setLookupOptions((previous) => ({
              ...previous,
              [field.name]: options
            }));
          })
          .finally(() => {
            setLookupLoading((previous) => ({ ...previous, [field.name]: false }));
          });
      });
    }, 300);

    return () => clearTimeout(timer);
  }, [formValues, lookupFields, lookupInput, open]);

  const handleSubmit = async () => {
    const payload: Record<string, unknown> = {};

    definition.formFields.forEach((field) => {
      payload[field.name] = parseValue(field, formValues[field.name]);
    });

    if (isEdit && initialData?.id) {
      payload.id = initialData.id;
    }

    await onSubmit(payload);
  };

  return (
    <Dialog open={open} fullWidth maxWidth="md" onClose={loading ? undefined : onClose}>
      <DialogTitle>{isEdit ? `Editar ${definition.title}` : `Novo ${definition.title}`}</DialogTitle>
      <DialogContent>
        <Stack spacing={2} mt={1}>
          {definition.formFields.map((field) => {
            if (field.type === "checkbox") {
              return (
                <FormControlLabel
                  key={field.name}
                  control={
                    <Checkbox
                      checked={Boolean(formValues[field.name])}
                      onChange={(event) =>
                        setFormValues((previous) => ({
                          ...previous,
                          [field.name]: event.target.checked
                        }))
                      }
                    />
                  }
                  label={field.label}
                />
              );
            }

            if (field.type === "select") {
              return (
                <TextField
                  key={field.name}
                  label={field.label}
                  select
                  required={field.required}
                  value={String(formValues[field.name] ?? "")}
                  onChange={(event) =>
                    setFormValues((previous) => ({
                      ...previous,
                      [field.name]: event.target.value
                    }))
                  }
                >
                  {(field.options ?? []).map((option) => (
                    <MenuItem key={option} value={option}>
                      {option}
                    </MenuItem>
                  ))}
                </TextField>
              );
            }

            if (field.type === "lookup" && field.lookup) {
              const options = lookupOptions[field.name] ?? [];
              const selectedId = String(formValues[field.name] ?? "");
              const selectedOption = options.find((option) => option.id === selectedId) ?? null;

              return (
                <Autocomplete
                  key={field.name}
                  options={options}
                  value={selectedOption}
                  loading={Boolean(lookupLoading[field.name])}
                  getOptionLabel={(option) => option.label}
                  isOptionEqualToValue={(option, value) => option.id === value.id}
                  onChange={(_, option) => {
                    setFormValues((previous) => ({
                      ...previous,
                      [field.name]: option?.id ?? ""
                    }));
                  }}
                  inputValue={lookupInput[field.name] ?? ""}
                  onInputChange={(_, value) => {
                    setLookupInput((previous) => ({ ...previous, [field.name]: value }));
                  }}
                  renderInput={(params) => (
                    <TextField
                      {...params}
                      label={field.label}
                      required={field.required}
                      placeholder="Digite para buscar..."
                      InputProps={{
                        ...params.InputProps,
                        endAdornment: (
                          <>
                            {lookupLoading[field.name] ? <CircularProgress color="inherit" size={16} /> : null}
                            {params.InputProps.endAdornment}
                          </>
                        )
                      }}
                    />
                  )}
                />
              );
            }

            const textFieldType = field.type === "number" ? "number" : field.type === "datetime" ? "datetime-local" : field.type;

            return (
              <TextField
                key={field.name}
                label={field.label}
                required={field.required}
                type={textFieldType}
                value={String(formValues[field.name] ?? "")}
                onChange={(event) =>
                  setFormValues((previous) => ({
                    ...previous,
                    [field.name]: event.target.value
                  }))
                }
                InputLabelProps={field.type === "date" || field.type === "datetime" ? { shrink: true } : undefined}
              />
            );
          })}
        </Stack>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} disabled={loading}>
          Cancelar
        </Button>
        <Button variant="contained" onClick={() => void handleSubmit()} disabled={loading}>
          {loading ? "Salvando..." : "Salvar"}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
