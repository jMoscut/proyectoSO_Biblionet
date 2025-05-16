import { parseDate } from "@internationalized/date";
import { z } from "zod";
import { invalid_type_error, required_error } from "../config/constants";

export const documentShema = z.object({
  author: z
    .string({ invalid_type_error, required_error })
    .min(1, { message: "El autor es requerido" }),
  documentNumber: z
    .string({ invalid_type_error, required_error })
    .min(1, { message: "El número de documento es requerido" }),
  elaborationDate: z
    .string({ invalid_type_error, required_error })
    .min(1, { message: "La fecha de elaboración es requerida" })
    .refine(
      (date) => {
        if (date.trim() === "") {
          return true;
        }

        const today = new Date();
        const inputDate = parseDate(date);

        today.setHours(0, 0, 0, 0);
        const todayDate = parseDate(today.toISOString().split("T")[0]);

        return inputDate <= todayDate;
      },
      {
        message: "La fecha de elaboración no puede ser mayor a la fecha actual",
      },
    ),
  file: z
    .instanceof(File, { message: "El archivo debe ser un pdf" })
    .refine((file) => file.size < 5000000, {
      message: "El archivo debe pesar menos de 5MB",
    })
    .refine((file) => file.type === "application/pdf", {
      message: "El archivo debe ser un pdf",
    })
    .refine((file) => file.name.endsWith(".pdf"), {
      message: "El archivo debe tener una extensión .pdf",
    }),
});
