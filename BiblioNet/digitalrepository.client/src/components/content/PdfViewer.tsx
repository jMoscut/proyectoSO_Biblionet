import WebViewer from "@pdftron/webviewer";
import { useEffect, useRef } from "react";

export const PdfViewer = ({ fileBlob }: { fileBlob: Blob | string }) => {
  const viewerDiv = useRef<HTMLDivElement>(null);

  useEffect(() => {
    WebViewer(
      {
        path: "/lib",
        initialDoc: "",
      },
      viewerDiv.current as HTMLDivElement,
    ).then((instance) => {
      const { UI } = instance;
      UI.loadDocument(window.URL.createObjectURL(fileBlob as Blob), {
        filename: "document.pdf",
      });
      UI.setZoomLevel(1.5);
    });

    return () => {
      if (typeof fileBlob !== "string") {
        window.URL.revokeObjectURL(fileBlob as unknown as string);
      }
    };
  }, []);

  return <div className="webviewer" ref={viewerDiv}></div>;
};
