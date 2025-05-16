import { RouteObject } from "react-router";
import { nameRoutes } from "../config/constants";
import { DownloadPage } from "../pages/documents/DownloadPage";
import { ListDocumentsPage } from "../pages/documents/ListDocumentsPage";
import { LoadPage } from "../pages/documents/LoadPage";

export const DocumentRoutes: RouteObject[] = [
  {
    path: `${nameRoutes.documents}/${nameRoutes.download}`,
    element: <DownloadPage />,
  },
  {
    path: `${nameRoutes.documents}/${nameRoutes.create}`,
    element: <LoadPage />,
  },
  {
    path: nameRoutes.documents,
    element: <ListDocumentsPage />,
  },
];
