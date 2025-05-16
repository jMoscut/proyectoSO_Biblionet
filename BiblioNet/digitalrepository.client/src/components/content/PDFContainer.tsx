import { PdfViewer } from "./PdfViewer";

export const PdfContainer = ({
  pdfBlob,
}: {
  pdfBlob: Blob | null | string;
}) => {
  return <div>{pdfBlob && <PdfViewer fileBlob={pdfBlob} />}</div>;
};
