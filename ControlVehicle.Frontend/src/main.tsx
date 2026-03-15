import React from "react";
import ReactDOM from "react-dom/client";
import CssBaseline from "@mui/material/CssBaseline";
import { RouterProvider } from "react-router-dom";
import { AppProviders } from "./app/providers/AppProviders";
import { router } from "./app/router/router";

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <AppProviders>
      <CssBaseline />
      <RouterProvider router={router} />
    </AppProviders>
  </React.StrictMode>
);
