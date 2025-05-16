import { useCallback, useState } from "react";
import { useDropzone } from "react-dropzone";
import { FileUploaderProps } from "../components/content/FileUploader";
import { fileTypes } from "../config/constants";

export const useFileReader = ({ changeFile }: FileUploaderProps) => {
  const [fileContent, setFileContent] = useState<File | null>(null);

  const onDrop = useCallback((acceptedFiles: File[]) => {
    acceptedFiles.forEach((file) => {
      setFileContent(file);
      debugger;
      changeFile({
        name: "file",
        files: file,
      });
    });
  }, []);

  const { getRootProps, getInputProps, isDragActive } = useDropzone({
    onDrop,
    accept: fileTypes,
    disabled: fileContent !== null,
    multiple: false,
  });

  return { getRootProps, getInputProps, isDragActive, fileContent };
};
