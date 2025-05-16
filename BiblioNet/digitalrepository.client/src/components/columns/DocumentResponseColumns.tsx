import { DocumentResponse } from "../../types/DocumentResponse";
import { TableColumnWithFilters } from "../../types/TableColumnWithFilters";

export const DocumentResponseColumns: TableColumnWithFilters<DocumentResponse>[] =
  [
    {
      id: "id",
      name: "Id",
      selector: (data) => data.id ?? "",
      sortable: true,
      maxWidth: "150px",
      omit: true,
    },
    {
      id: "documentNumber",
      name: "Número de Documento",
      selector: (data) => data.documentNumber ?? "",
      sortable: true,
      wrap: true,
      omit: false,
      hasFilter: true,
      filterField: (value) => (value ? `DocumentNumber:like:${value}` : ""),
    },
    {
      id: "author",
      name: "Propietario",
      selector: (data) => data.author ?? "",
      sortable: true,
      wrap: true,
      omit: false,
      hasFilter: true,
      filterField: (value) => (value ? `Author:like:${value}` : ""),
    },
    {
      id: "path",
      name: "Ruta",
      selector: (data) => data.path ?? "",
      sortable: true,
      wrap: true,
      omit: false,
      hasFilter: true,
      filterField: (value) => (value ? `Path:like:${value}` : ""),
    },
    {
      id: "createdAt",
      name: "Creado",
      selector: (data) => data.createdAt ?? "",
      sortable: true,
      maxWidth: "160px",
      omit: true,
    },
    {
      id: "updatedAt",
      name: "Actualizado",
      selector: (data) => data.updatedAt ?? "",
      sortable: true,
      maxWidth: "160px",
      omit: true,
    },
  ];
