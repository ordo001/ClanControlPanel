import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  // base: "/",
  // build: {
  //   outDir: "dist",
  // },
  // server: {
  //   allowedHosts: ["flogritunnel.com"],
  // },
  // server: {
  //   host: true,
  //   port: 5173,
  //   strictPort: true,
  //   cors: true,
  //   allowedHosts: "all",
  // },
});
