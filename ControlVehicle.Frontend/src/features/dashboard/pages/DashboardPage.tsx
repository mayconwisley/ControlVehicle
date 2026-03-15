import { useEffect, useState } from "react";
import { Alert, Card, CardContent, Grid, Stack, Typography } from "@mui/material";
import { Link } from "react-router-dom";
import { getResourcePage } from "../../../shared/api/client";
import type { ResourceSummary } from "../../../shared/types/api";
import { resourceDefinitions } from "../../resources/config/resources";

export const DashboardPage = () => {
  const [summary, setSummary] = useState<ResourceSummary[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadSummary = async () => {
      setError(null);

      try {
        const result = await Promise.all(
          resourceDefinitions.map(async (resource) => {
            const response = await getResourcePage(resource.endpoint, 1, 1, "");
            return {
              key: resource.key,
              title: resource.title,
              total: response.totalData
            } satisfies ResourceSummary;
          })
        );

        setSummary(result);
      } catch {
        setError("Nao foi possivel carregar os indicadores iniciais.");
      }
    };

    void loadSummary();
  }, []);

  return (
    <Stack spacing={2}>
      <Stack spacing={0.5}>
        <Typography variant="h4">Painel de Frota</Typography>
        <Typography variant="body2" color="text.secondary">
          Visao geral dos modulos de controle da frota.
        </Typography>
      </Stack>

      {error ? <Alert severity="warning">{error}</Alert> : null}

      <Grid container spacing={2}>
        {resourceDefinitions.map((resource) => {
          const item = summary.find((entry) => entry.key === resource.key);

          return (
            <Grid key={resource.key} size={{ xs: 12, sm: 6, lg: 4 }}>
              <Card component={Link} to={resource.route} sx={{ textDecoration: "none" }}>
                <CardContent>
                  <Stack direction="row" justifyContent="space-between" alignItems="center">
                    <Typography variant="subtitle1" color="text.secondary">
                      {resource.title}
                    </Typography>
                    {resource.icon}
                  </Stack>
                  <Typography variant="h5" mt={1}>
                    {item?.total ?? "--"}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          );
        })}
      </Grid>
    </Stack>
  );
};
