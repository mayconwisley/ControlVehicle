import { PropsWithChildren, useMemo, useState } from "react";
import { ThemeProvider } from "@mui/material/styles";
import type { PaletteMode } from "@mui/material/styles";
import { ColorModeContext } from "../theme/colorModeContext";
import { createAppTheme } from "../theme/createAppTheme";

const THEME_STORAGE_KEY = "control-vehicle-theme";

const getInitialMode = (): PaletteMode => {
  const savedMode = localStorage.getItem(THEME_STORAGE_KEY);
  if (savedMode === "light" || savedMode === "dark") {
    return savedMode;
  }

  return window.matchMedia("(prefers-color-scheme: dark)").matches ? "dark" : "light";
};

export const AppProviders = ({ children }: PropsWithChildren) => {
  const [mode, setMode] = useState<PaletteMode>(getInitialMode);

  const colorMode = useMemo(
    () => ({
      mode,
      toggleColorMode: () => {
        setMode((previousMode) => {
          const nextMode: PaletteMode = previousMode === "light" ? "dark" : "light";
          localStorage.setItem(THEME_STORAGE_KEY, nextMode);
          return nextMode;
        });
      }
    }),
    [mode]
  );

  const theme = useMemo(() => createAppTheme(mode), [mode]);

  return (
    <ColorModeContext.Provider value={colorMode}>
      <ThemeProvider theme={theme}>{children}</ThemeProvider>
    </ColorModeContext.Provider>
  );
};
