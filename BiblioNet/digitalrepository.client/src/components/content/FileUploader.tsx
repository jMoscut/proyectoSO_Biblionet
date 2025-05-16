import { useFileReader } from "../../hooks/useFileReader";
import { FileObject } from "../../hooks/useForm";
import { DropZone } from "../pure/DropZone";

export interface FileUploaderProps {
  changeFile: (file: FileObject) => void;
}

export const FileUploader = ({ changeFile }: FileUploaderProps) => {
  const { getInputProps, getRootProps, isDragActive, fileContent } =
    useFileReader({ changeFile });

  return (
    <div className="w-full px-1 h-[20vh] overflow-auto">
      <div {...getRootProps()}>
        <input {...getInputProps()} />
        <DropZone
          isDragActive={isDragActive}
          isFileContent={fileContent != null}
          isError={false}
        />
      </div>
    </div>
  );
};
