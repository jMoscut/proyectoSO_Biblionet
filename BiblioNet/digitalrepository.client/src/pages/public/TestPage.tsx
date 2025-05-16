import { Image } from "@heroui/image";
import { Images } from "../../assets/images/images";
import { useAuth } from "../../hooks/useAuth";

export const TestPage = () => {
  const { email } = useAuth();

  return (
    <div className="page-view flex flex-col gap-10 items-center justify-center bg-gray-100">
      <h1 className="font-bold text-2xl">Bienvenido de Nuevo {email}</h1>
      <Image
        radius="full"
        className="border-2 border-gray-300"
        isZoomed
        isBlurred
        alt="logo"
        src={Images.logo}
        width={300}
        height={300}
      />
    </div>
  );
};
