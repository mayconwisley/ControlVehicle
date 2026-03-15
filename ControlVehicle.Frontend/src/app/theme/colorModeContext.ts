import { createContext } from "react";
import type { PaletteMode } from "@mui/material/styles";

export type ColorModeContextType = {
  mode: PaletteMode;
  toggleColorMode: () => void;
};

export const ColorModeContext = createContext<ColorModeContextType>({
  mode: "light",
  toggleColorMode: () => undefined
});
