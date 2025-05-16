import { GenericFormData, toFormData } from "axios";
import { api } from "../config/axios/interceptors";
import { DocumentRequest } from "../pages/documents/LoadPage";
import { ApiResponse } from "../types/ApiResponse";
import { DocumentResponse } from "../types/DocumentResponse";
import { ValidationFailure } from "../types/ValidationFailure";

export const loadDocumetToServer = async (
  form: DocumentRequest,
): Promise<ApiResponse<any>> => {
  const response = await api.post<
    any,
    ApiResponse<DocumentResponse>,
    GenericFormData
  >("Document", toFormData(form), {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });

  return response;
};

export const getDocuments = async (
  filter?: string,
  page = 1,
  pageSize = 10,
): Promise<ApiResponse<DocumentResponse[]>> => {
  let response: ApiResponse<DocumentResponse[]>;

  if (filter) {
    response = await api.get<any, ApiResponse<DocumentResponse[]>>(
      `Document?filters=${filter}&pageNumber=${page}&pageSize=${pageSize}`,
    );
  } else {
    response = await api.get<object, ApiResponse<DocumentResponse[]>>(
      `Document?pageNumber=${page}&pageSize=${pageSize}`,
    );
  }

  return response;
};

export const downloadPdf = async (id: number) => {
  try {
    // Hacer la solicitud GET a la API con el path del archivo
    const response = await api.get<
      any,
      Blob | ApiResponse<ValidationFailure[]>
    >(`document/download/${id}`, {
      responseType: "blob", // Aseguramos que la respuesta sea un blob (archivo binario)
    });

    // Verificar si la respuesta es un blob
    if (!(response instanceof Blob && response.type === "application/pdf")) {
      console.error("Error al descargar el archivo:", response);
      return { success: false, pdf: null };
    }

    // Verificar si la respuesta es un error
    if ((response as any)?.success) {
      console.error("Error al descargar el archivo:", response);
      return { success: false, pdf: null };
    }

    // Crear un enlace para descargar el archivo
    const blob = new Blob([response], { type: "application/pdf" });

    return {
      success: true,
      pdf: blob,
    };
  } catch (error) {
    console.error("Error al descargar el archivo:", error);
    return {
      success: false,
      pdf: null,
    };
  }
};

export const downloadPdfs = async (filters: string) => {
  try {
    // Hacer la solicitud GET a la API con el path del archivo
    const response = await api.get<
      any,
      Blob | ApiResponse<ValidationFailure[]>
    >(`document/download?filters=${filters}`, {
      responseType: "blob", // Aseguramos que la respuesta sea un blob (archivo binario)
    });

    // Verificar si la respuesta es un blob
    if (!(response instanceof Blob && response.type === "application/zip")) {
      console.error("Error al descargar el archivo:", response);
      return false;
    }

    // Verificar si la respuesta es un error
    if ((response as any)?.success) {
      console.error("Error al descargar el archivo:", response);
      return false;
    }

    // Crear un enlace para descargar el archivo
    const blob = new Blob([response], { type: "application/zip" });

    // Crear una URL para el blob
    const downloadUrl = window.URL.createObjectURL(blob);

    // Crear un elemento <a> temporal para descargar el archivo
    const link = document.createElement("a");
    link.href = downloadUrl;
    link.setAttribute("download", "documents.zip"); // Nombre del archivo a descargar

    // Hacer clic en el enlace para descargar
    document.body.appendChild(link);
    link.click();

    // Limpiar el enlace
    document.body.removeChild(link);
    window.URL.revokeObjectURL(downloadUrl); // Liberar la URL del objeto

    return true;
  } catch (error) {
    console.error("Error al descargar el archivo:", error);
    return false;
  }
};
