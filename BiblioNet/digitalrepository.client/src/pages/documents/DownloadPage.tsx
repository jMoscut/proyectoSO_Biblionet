import { Button } from "@heroui/button";
import {
  Modal,
  ModalBody,
  ModalContent,
  ModalFooter,
  ModalHeader,
} from "@heroui/modal";
import { addToast } from "@heroui/toast";
import { useState } from "react";
import { DownloadDocumentColumns } from "../../components/columns/DownloadDocumentColumns";
import { PdfContainer } from "../../components/content/PDFContainer";
import { Icon } from "../../components/icons/Icon";
import { TableServer } from "../../components/tables/TableServer";
import Protected from "../../routes/middlewares/Protected";
import { downloadPdfs, getDocuments } from "../../services/documentService";
import { useDocumentStore } from "../../stores/useDocumentStore";
import { compactGrid } from "../../theme/tableTheme";

export const DownloadPage = () => {
  const [downloadDocuments, setDownloadDocuments] = useState<number[]>([]);
  const {
    documentFilters,
    setDocumentFilters,
    pdfBlob,
    openDrawer,
    setOpenDrawer,
  } = useDocumentStore();

  const handleDownload = async () => {
    //filter format Id:eq:1 OR Id:eq:3 OR Id:eq:11
    const filter = downloadDocuments.map((id) => `Id:eq:${id}`).join(" OR ");

    const response = await downloadPdfs(filter);
    if (response) {
      addToast({
        title: "Success",
        description: "Documentos descargados correctamente",
        icon: "bi bi-exclamation-triangle-fill",
        color: "success",
        timeout: 5000,
      });
    } else {
      addToast({
        title: "Error",
        description: "Error al descargar los documentos",
        icon: "bi bi-exclamation-triangle-fill",
        color: "danger",
        timeout: 5000,
      });
    }
  };

  return (
    <Protected>
      <div className="px-10">
        <h1 className="text-center font-bold text-2xl">
          Documentos Descargables
        </h1>
        <div className="flex justify-end mb-4">
          {downloadDocuments.length > 0 && (
            <Button variant="shadow" color="primary" onPress={handleDownload}>
              <Icon name="bi bi-cloud-arrow-down-fill" /> Descargar Documentos
              Seleccionados
            </Button>
          )}
        </div>
        <TableServer
          hasRangeOfDates
          hasFilters
          selectedRows={true}
          onSelectedRowsChange={(state) => {
            setDownloadDocuments(state.selectedRows.map((row) => row.id));
          }}
          fieldRangeOfDates="CreatedAt"
          columns={DownloadDocumentColumns}
          filters={documentFilters}
          queryFn={getDocuments}
          queryKey={"documents"}
          setFilters={setDocumentFilters}
          styles={compactGrid}
          text="documentos"
          title="Documentos"
        />
        <Modal
          isOpen={openDrawer}
          onOpenChange={() => setOpenDrawer(!openDrawer)}
          size="full"
        >
          <ModalContent className="h-max-w-4xl overflow-y-auto">
            {(onClose) => (
              <>
                <ModalHeader className="flex flex-col gap-1">
                  Visor PDF
                </ModalHeader>
                <ModalBody>
                  <PdfContainer pdfBlob={pdfBlob} />
                </ModalBody>
                <ModalFooter>
                  <Button color="danger" variant="light" onPress={onClose}>
                    Cerrar
                  </Button>
                </ModalFooter>
              </>
            )}
          </ModalContent>
        </Modal>
      </div>
    </Protected>
  );
};
