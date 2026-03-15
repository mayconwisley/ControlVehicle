import { useContext, useMemo, useState } from "react";
import {
  AppBar,
  Box,
  Divider,
  Drawer,
  IconButton,
  List,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Stack,
  Toolbar,
  Typography
} from "@mui/material";
import { DarkMode as DarkModeIcon, Dashboard as DashboardIcon, LightMode as LightModeIcon, Menu as MenuIcon } from "@mui/icons-material";
import { NavLink, Outlet } from "react-router-dom";
import { ColorModeContext } from "../theme/colorModeContext";
import { resourceDefinitions } from "../../features/resources/config/resources";

const drawerWidth = 260;

export const AppLayout = () => {
  const [mobileOpen, setMobileOpen] = useState(false);
  const { mode, toggleColorMode } = useContext(ColorModeContext);

  const navigationItems = useMemo(
    () => [
      {
        label: "Dashboard",
        to: "/",
        icon: <DashboardIcon fontSize="small" />
      },
      ...resourceDefinitions.map((resource) => ({
        label: resource.title,
        to: resource.route,
        icon: resource.icon
      }))
    ],
    []
  );

  const drawer = (
    <Box>
      <Toolbar>
        <Stack>
          <Typography variant="h6">Controle Veiculo</Typography>
          <Typography variant="caption" color="text.secondary">
            Frontend React + MUI
          </Typography>
        </Stack>
      </Toolbar>
      <Divider />
      <List>
        {navigationItems.map((item) => (
          <ListItemButton
            key={item.to}
            component={NavLink}
            to={item.to}
            onClick={() => setMobileOpen(false)}
            sx={{
              "&.active": {
                backgroundColor: "action.selected"
              }
            }}
          >
            <ListItemIcon>{item.icon}</ListItemIcon>
            <ListItemText primary={item.label} />
          </ListItemButton>
        ))}
      </List>
    </Box>
  );

  return (
    <Box sx={{ display: "flex", minHeight: "100vh" }}>
      <AppBar position="fixed" sx={{ width: { md: `calc(100% - ${drawerWidth}px)` }, ml: { md: `${drawerWidth}px` } }}>
        <Toolbar>
          <IconButton color="inherit" edge="start" onClick={() => setMobileOpen(!mobileOpen)} sx={{ mr: 1, display: { md: "none" } }}>
            <MenuIcon />
          </IconButton>
          <Typography variant="h6" sx={{ flexGrow: 1 }}>
            Gestao de Frota
          </Typography>
          <IconButton color="inherit" onClick={toggleColorMode}>
            {mode === "light" ? <DarkModeIcon /> : <LightModeIcon />}
          </IconButton>
        </Toolbar>
      </AppBar>

      <Box component="nav" sx={{ width: { md: drawerWidth }, flexShrink: { md: 0 } }}>
        <Drawer
          variant="temporary"
          open={mobileOpen}
          onClose={() => setMobileOpen(false)}
          ModalProps={{ keepMounted: true }}
          sx={{
            display: { xs: "block", md: "none" },
            "& .MuiDrawer-paper": { boxSizing: "border-box", width: drawerWidth }
          }}
        >
          {drawer}
        </Drawer>
        <Drawer
          variant="permanent"
          open
          sx={{
            display: { xs: "none", md: "block" },
            "& .MuiDrawer-paper": { boxSizing: "border-box", width: drawerWidth }
          }}
        >
          {drawer}
        </Drawer>
      </Box>

      <Box component="main" sx={{ flexGrow: 1, p: 3, mt: 8 }}>
        <Outlet />
      </Box>
    </Box>
  );
};
