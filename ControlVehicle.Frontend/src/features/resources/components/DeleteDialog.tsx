import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Typography } from "@mui/material";

type DeleteDialogProps = {
  open: boolean;
  loading: boolean;
  title: string;
  description: string;
  onClose: () => void;
  onConfirm: () => Promise<void>;
};

export const DeleteDialog = ({ open, loading, title, description, onClose, onConfirm }: DeleteDialogProps) => (
  <Dialog open={open} onClose={loading ? undefined : onClose} fullWidth maxWidth="xs">
    <DialogTitle>{title}</DialogTitle>
    <DialogContent>
      <Typography variant="body2" color="text.secondary">
        {description}
      </Typography>
    </DialogContent>
    <DialogActions>
      <Button onClick={onClose} disabled={loading}>
        Cancelar
      </Button>
      <Button color="error" variant="contained" onClick={() => void onConfirm()} disabled={loading}>
        {loading ? "Excluindo..." : "Excluir"}
      </Button>
    </DialogActions>
  </Dialog>
);
