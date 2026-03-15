import {
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography
} from "@mui/material";
import { DeleteOutline as DeleteIcon, EditOutlined as EditIcon } from "@mui/icons-material";
import type { ResourceColumn } from "../config/resources";

type ResourceTableProps = {
  columns: ResourceColumn[];
  rows: Record<string, unknown>[];
  onEdit: (row: Record<string, unknown>) => void;
  onDelete: (row: Record<string, unknown>) => void;
};

export const ResourceTable = ({ columns, rows, onEdit, onDelete }: ResourceTableProps) => (
  <TableContainer component={Paper} variant="outlined">
    <Table size="small">
      <TableHead>
        <TableRow>
          {columns.map((column) => (
            <TableCell key={String(column.field)}>{column.headerName}</TableCell>
          ))}
          <TableCell align="right">Ações</TableCell>
        </TableRow>
      </TableHead>
      <TableBody>
        {rows.length === 0 ? (
          <TableRow>
            <TableCell colSpan={columns.length + 1}>
              <Typography variant="body2" color="text.secondary">
                Nenhum registro encontrado para este filtro.
              </Typography>
            </TableCell>
          </TableRow>
        ) : (
          rows.map((row, index) => (
            <TableRow key={index} hover>
              {columns.map((column) => {
                const value = row[column.field];
                return (
                  <TableCell key={String(column.field)}>
                    {column.render ? column.render(value, row) : String(value ?? "-")}
                  </TableCell>
                );
              })}
              <TableCell align="right">
                <IconButton size="small" color="primary" onClick={() => onEdit(row)}>
                  <EditIcon fontSize="small" />
                </IconButton>
                <IconButton size="small" color="error" onClick={() => onDelete(row)}>
                  <DeleteIcon fontSize="small" />
                </IconButton>
              </TableCell>
            </TableRow>
          ))
        )}
      </TableBody>
    </Table>
  </TableContainer>
);
