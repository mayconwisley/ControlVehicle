import { createTheme, responsiveFontSizes, type PaletteMode } from "@mui/material/styles";

export const createAppTheme = (mode: PaletteMode) => {
  const theme = createTheme({
    palette: {
      mode,
      primary: {
        main: mode === "light" ? "#0057d9" : "#7eb0ff"
      },
      secondary: {
        main: mode === "light" ? "#00a87e" : "#00c49a"
      },
      background: {
        default: mode === "light" ? "#f4f6fb" : "#0f1724",
        paper: mode === "light" ? "#ffffff" : "#172235"
      }
    },
    shape: {
      borderRadius: 12
    },
    typography: {
      fontFamily: "'Segoe UI', 'Roboto', 'Helvetica Neue', Arial, sans-serif",
      h4: {
        fontWeight: 700
      }
    },
    components: {
      MuiCard: {
        styleOverrides: {
          root: {
            borderRadius: 16
          }
        }
      }
    }
  });

  return responsiveFontSizes(theme);
};
