import { DocumentResponseColumns } from "../../components/columns/DocumentResponseColumns";
import { TableServer } from "../../components/tables/TableServer";
import Protected from "../../routes/middlewares/Protected";
import { getDocuments } from "../../services/documentService";
import { useDocumentStore } from "../../stores/useDocumentStore";
import { compactGrid } from "../../theme/tableTheme";

export const ListDocumentsPage = () => {
  const { documentFilters, setDocumentFilters } = useDocumentStore();

  return (
    <Protected>
      <div className="px-10">
        <h1 className="text-center font-bold text-2xl">
          Listado de Documentos
        </h1>
        <TableServer
          hasRangeOfDates
          hasFilters
          fieldRangeOfDates="CreatedAt"
          columns={DocumentResponseColumns}
          filters={documentFilters}
          queryFn={getDocuments}
          queryKey={"documents"}
          setFilters={setDocumentFilters}
          styles={compactGrid}
          text="documentos"
          title="Documentos"
        />
      </div>
    </Protected>
  );
};
