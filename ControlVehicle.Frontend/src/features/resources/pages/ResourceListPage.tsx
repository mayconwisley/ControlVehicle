import { useCallback, useEffect, useMemo, useState } from "react";
import { Add as AddIcon } from "@mui/icons-material";
import { Alert, Box, Button, Card, CardContent, Pagination, Stack, TextField, Typography } from "@mui/material";
import { useParams } from "react-router-dom";
import { createResource, deleteResource, getResourcePage, updateResource } from "../../../shared/api/client";
import type { PaginatedResponse } from "../../../shared/types/api";
import { DeleteDialog } from "../components/DeleteDialog";
import { ResourceFormDialog } from "../components/ResourceFormDialog";
import { ResourceTable } from "../components/ResourceTable";
import { resourceDefinitions } from "../config/resources";

export const ResourceListPage = () => {
  const { resourceKey } = useParams();

  const definition = useMemo(
    () => resourceDefinitions.find((item) => item.slug.toLowerCase() === resourceKey?.toLowerCase()),
    [resourceKey]
  );

  const [search, setSearch] = useState("");
  const [page, setPage] = useState(1);
  const [data, setData] = useState<PaginatedResponse<Record<string, unknown>>>({
    totalData: 0,
    page: 1,
    totalPage: 1,
    size: 5,
    items: []
  });
  const [error, setError] = useState<string | null>(null);
  const [feedback, setFeedback] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const [formOpen, setFormOpen] = useState(false);
  const [isEdit, setIsEdit] = useState(false);
  const [selectedRow, setSelectedRow] = useState<Record<string, unknown> | null>(null);

  const [deleteOpen, setDeleteOpen] = useState(false);
  const [pendingDelete, setPendingDelete] = useState<Record<string, unknown> | null>(null);
  const [saving, setSaving] = useState(false);
  const [deleting, setDeleting] = useState(false);

  const loadData = useCallback(async () => {
    if (!definition) {
      return;
    }

    setLoading(true);
    setError(null);

    try {
      const response = await getResourcePage<Record<string, unknown>>(definition.endpoint, page, 5, search);
      setData(response);
    } catch (requestError) {
      const fallbackMessage = "Nao foi possivel carregar os dados.";
      setError(requestError instanceof Error ? requestError.message : fallbackMessage);
    } finally {
      setLoading(false);
    }
  }, [definition, page, search]);

  useEffect(() => {
    setPage(1);
  }, [resourceKey]);

  useEffect(() => {
    const timer = setTimeout(() => {
      void loadData();
    }, 250);

    return () => clearTimeout(timer);
  }, [loadData]);

  const handleCreateClick = () => {
    setSelectedRow(null);
    setIsEdit(false);
    setFormOpen(true);
  };

  const handleEditClick = (row: Record<string, unknown>) => {
    setSelectedRow(row);
    setIsEdit(true);
    setFormOpen(true);
  };

  const handleDeleteClick = (row: Record<string, unknown>) => {
    setPendingDelete(row);
    setDeleteOpen(true);
  };

  const handleSubmit = async (payload: Record<string, unknown>) => {
    if (!definition) {
      return;
    }

    setSaving(true);
    setError(null);

    try {
      if (isEdit) {
        const id = String(payload.id ?? "");
        await updateResource(definition.endpoint, encodeURIComponent(id), payload);
        setFeedback("Registro atualizado com sucesso.");
      } else {
        await createResource(definition.endpoint, payload);
        setFeedback("Registro criado com sucesso.");
      }

      setFormOpen(false);
      await loadData();
    } catch (submitError) {
      const fallbackMessage = "Nao foi possivel salvar o registro.";
      setError(submitError instanceof Error ? submitError.message : fallbackMessage);
    } finally {
      setSaving(false);
    }
  };

  const confirmDelete = async () => {
    if (!definition || !pendingDelete) {
      return;
    }

    const deleteFieldValue = pendingDelete[definition.deleteByField];
    if (!deleteFieldValue) {
      setError("Nao foi possivel determinar o identificador para exclusao.");
      return;
    }

    setDeleting(true);
    setError(null);

    try {
      await deleteResource(definition.endpoint, encodeURIComponent(String(deleteFieldValue)));
      setFeedback("Registro excluido com sucesso.");
      setDeleteOpen(false);
      setPendingDelete(null);
      await loadData();
    } catch (deleteError) {
      const fallbackMessage = "Nao foi possivel excluir o registro.";
      setError(deleteError instanceof Error ? deleteError.message : fallbackMessage);
    } finally {
      setDeleting(false);
    }
  };

  if (!definition) {
    return <Alert severity="warning">Recurso nao encontrado. Selecione uma opcao valida no menu lateral.</Alert>;
  }

  return (
    <Stack spacing={2}>
      <Box display="flex" justifyContent="space-between" alignItems="center" gap={2} flexWrap="wrap">
        <Box>
          <Typography variant="h4">{definition.title}</Typography>
          <Typography variant="body2" color="text.secondary">
            CRUD completo do modulo com integracao direta na API.
          </Typography>
        </Box>
        <Button variant="contained" startIcon={<AddIcon />} onClick={handleCreateClick}>
          Novo registro
        </Button>
      </Box>

      <Card>
        <CardContent>
          <Stack direction={{ xs: "column", md: "row" }} spacing={2} alignItems={{ md: "center" }}>
            <TextField
              label="Buscar"
              value={search}
              fullWidth
              onChange={(event) => setSearch(event.target.value)}
              placeholder="Digite para filtrar"
            />
            <Typography variant="body2" color="text.secondary" minWidth={180}>
              Total de registros: {data.totalData}
            </Typography>
          </Stack>
        </CardContent>
      </Card>

      {error ? <Alert severity="error">{error}</Alert> : null}
      {feedback ? <Alert severity="success" onClose={() => setFeedback(null)}>{feedback}</Alert> : null}

      {loading ? (
        <Alert severity="info">Carregando dados...</Alert>
      ) : (
        <ResourceTable columns={definition.columns} rows={data.items} onEdit={handleEditClick} onDelete={handleDeleteClick} />
      )}

      <Box display="flex" justifyContent="flex-end">
        <Pagination color="primary" count={Math.max(1, data.totalPage)} page={page} onChange={(_, nextPage) => setPage(nextPage)} />
      </Box>

      <ResourceFormDialog
        open={formOpen}
        isEdit={isEdit}
        loading={saving}
        definition={definition}
        initialData={selectedRow}
        onClose={() => setFormOpen(false)}
        onSubmit={handleSubmit}
      />

      <DeleteDialog
        open={deleteOpen}
        loading={deleting}
        title={`Excluir ${definition.title}`}
        description="Esta acao remove o registro permanentemente. Confirme para continuar."
        onClose={() => {
          setDeleteOpen(false);
          setPendingDelete(null);
        }}
        onConfirm={confirmDelete}
      />
    </Stack>
  );
};
