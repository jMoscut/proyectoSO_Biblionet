import { AiFillFileText } from "react-icons/ai";
import { IoIosCloudDone } from "react-icons/io";
import { RiCloudOffFill, RiFileAddLine, RiFileCloudLine } from "react-icons/ri";

interface DropZoneProps {
  isDragActive: boolean;
  isFileContent: boolean;
  isError?: boolean;
}

export const DropZone = ({
  isDragActive,
  isFileContent,
  isError,
}: DropZoneProps) => {
  return !isFileContent ? (
    <section className="border-3 border-gray-300 border-dashed py-6 ">
      {isDragActive ? (
        <article className="flex gap-4 justify-center items-center">
          <RiFileAddLine className="text-gray-600" size={35} />
          <p>Suelte el archivo aquí...</p>
        </article>
      ) : (
        <article className="flex gap-4 justify-center items-center">
          <AiFillFileText className="text-gray-600" size={35} />
          <p>
            Arrastre y suelte algún .pdf aquí o haga clic para seleccionarlo
          </p>
        </article>
      )}
    </section>
  ) : (
    <section>
      {!isError ? (
        <article className="flex gap-4 justify-center items-center">
          <RiFileCloudLine size={35} className="text-gray-600" />
          <p className="text-2xl font-bold underline">
            Archivo cargado correctamente
          </p>
          <IoIosCloudDone size={35} className="text-gray-600" />
        </article>
      ) : (
        <article className="flex gap-4 justify-center items-center">
          <RiFileCloudLine size={35} className="text-gray-600" />
          <p className="text-2xl font-bold underline">
            Error al cargar el archivo
          </p>
          <RiCloudOffFill size={35} className="text-gray-600" />
        </article>
      )}
    </section>
  );
};
