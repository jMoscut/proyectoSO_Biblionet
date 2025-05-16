import { create } from "zustand";

import { ListFilter } from "../types/LIstFilter";

interface DocumentState {
  documentFilters: ListFilter;
  pdfBlob: Blob | null | string;
  openDrawer: boolean;
  setOpenDrawer: (openDrawer: boolean) => void;
  setPdfBlob: (pdfBlob: Blob | null | string) => void;
  setDocumentFilters: (documentFilters: ListFilter) => void;
}

export const useDocumentStore = create<DocumentState>((set) => ({
  documentFilters: { filter: "", page: 1, pageSize: 10 },
  setDocumentFilters: (documentFilters) => set({ documentFilters }),
  pdfBlob: null,
  setPdfBlob: (pdfBlob) => set({ pdfBlob }),
  openDrawer: false,
  setOpenDrawer: (openDrawer) => set({ openDrawer }),
}));
