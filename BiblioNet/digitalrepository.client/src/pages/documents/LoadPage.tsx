import { Button } from "@heroui/button";
import { DatePicker } from "@heroui/date-picker";
import { Input } from "@heroui/input";
import { addToast } from "@heroui/toast";
import { parseDate } from "@internationalized/date";
import { Response } from "../../components/messages/Response";
import { ErrorObject, useForm } from "../../hooks/useForm";
import ProtectedRoute from "../../routes/middlewares/ProtectedRoute";
import { loadDocumetToServer } from "../../services/documentService";
import { ApiResponse } from "../../types/ApiResponse";
import {
  formatCalendarDate,
  handleOneLevelZodError,
} from "../../utils/converted";
import { documentShema } from "../../validations/documentValidations";

export interface DocumentRequest {
  author: string;
  documentNumber: string;
  elaborationDate: string;
  file: File | null;
}

const initialForm: DocumentRequest = {
  author: "",
  documentNumber: "",
  elaborationDate: "",
  file: null,
};

const validateDocument = (form: DocumentRequest) => {
  let errors: ErrorObject = {};

  const parse = documentShema.safeParse(form);

  if (!parse.success) {
    errors = handleOneLevelZodError(parse.error);
  }

  return errors;
};

export const LoadPage = () => {
  const petition = async (form: DocumentRequest): Promise<ApiResponse<any>> => {
    const response = await loadDocumetToServer(form);

    console.log("response", response);

    if (response.success) {
      addToast({
        title: "Success",
        description: response.message,
        icon: "bi bi-exclamation-triangle-fill",
        color: "success",
        timeout: 1000,
      });
    } else {
      addToast({
        title: "Error",
        description: response.message,
        icon: "bi bi-exclamation-triangle-fill",
        color: "danger",
        timeout: 1000,
      });
    }

    return response;
  };

  const {
    errors,
    form,
    handleChangeFile,
    handleSubmit,
    handleChange,
    loading,
    success,
    message,
  } = useForm<DocumentRequest, any>(
    initialForm,
    validateDocument,
    petition,
    true,
  );

  return (
    <ProtectedRoute operation={2}>
      <section className="page-view flex flex-col justify-center items-center bg-gray-100">
        <form
          onSubmit={handleSubmit}
          className="w-full max-w-2xl flex flex-col gap-4 p-4 bg-white shadow-md rounded-lg"
        >
          {success != null && <Response message={message} type={success} />}
          <h1 className="text-center text-2xl font-bold">
            Carga de documentos
          </h1>
          <Input
            className={"py-4"}
            errorMessage={errors?.author}
            id={"author"}
            isInvalid={!!errors?.author}
            isRequired={true}
            label={"Propietario del documento"}
            name={"author"}
            size="lg"
            type={"text"}
            value={form.author}
            variant="bordered"
            onChange={handleChange}
          />
          <Input
            className={"py-4"}
            errorMessage={errors?.documentNumber}
            id={"documentNumber"}
            isInvalid={!!errors?.documentNumber}
            isRequired={true}
            label={"Número de documento"}
            name={"documentNumber"}
            size="lg"
            type={"text"}
            value={form.documentNumber}
            variant="bordered"
            onChange={handleChange}
          />
          <DatePicker
            className="py-4"
            errorMessage={errors?.elaborationDate}
            id="elaborationDate"
            isInvalid={!!errors?.elaborationDate}
            isRequired={true}
            value={
              form.elaborationDate ? parseDate(form.elaborationDate) : null
            }
            onChange={(value) => {
              handleChange({
                target: {
                  name: "elaborationDate",
                  value: formatCalendarDate(value!),
                } as any,
              } as React.ChangeEvent<HTMLInputElement>);
            }}
            size="lg"
            variant="bordered"
            label="Fecha de elaboracion"
            name="elaborationDate"
          />
          <Input
            className={"py-4"}
            errorMessage={errors?.file}
            id={"file"}
            isInvalid={!!errors?.file}
            isRequired={true}
            label={"Documento"}
            name={"file"}
            size="lg"
            type={"file"}
            variant="bordered"
            accept="application/pdf"
            onChange={handleChangeFile}
          />
          <Button
            type="submit"
            variant="solid"
            color="primary"
            isLoading={loading}
          >
            Guardar Documento
          </Button>
        </form>
      </section>
    </ProtectedRoute>
  );
};
