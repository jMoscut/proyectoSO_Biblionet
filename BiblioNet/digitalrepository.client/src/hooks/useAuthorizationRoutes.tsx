import { createBrowserRouter } from "react-router";

import { nameRoutes } from "../config/constants";
import { Root } from "../containers/Root";
import { ErrorRoutes } from "../routes/ErrorRoutes";
import { PublicRoutes } from "../routes/PublicRoutes";

import { DocumentRoutes } from "../routes/DocumentRoutes";
import { useAuth } from "./useAuth";

export const useAuthorizationRoutes = () => {
  const { allOperations } = useAuth();

  const routes = createBrowserRouter([
    {
      path: nameRoutes.root,
      element: <Root />,
      children: [...DocumentRoutes],
    },
    {
      path: nameRoutes.root,
      element: <Root />,
      children: [...PublicRoutes, ...ErrorRoutes],
    },
  ]);

  const operations = new Set(
    allOperations.map((operation) => operation.path.toLowerCase()),
  );

  const routesFiltered = routes.routes[0].children?.filter((route) => {
    return operations.has(route.path ?? "");
  });

  routes.routes[0].children = routesFiltered as any;

  return routes;
};
