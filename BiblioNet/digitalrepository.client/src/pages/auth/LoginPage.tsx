import { Button } from "@heroui/button";
import { Card, CardBody } from "@heroui/card";
import { Form } from "@heroui/form";
import { Input } from "@heroui/input";

import { Image } from "@heroui/image";
import { Images } from "../../assets/images/images";
import { Icon } from "../../components/icons/Icon";
import { Response } from "../../components/messages/Response";
import { useAuth } from "../../hooks/useAuth";
import { ErrorObject, useForm } from "../../hooks/useForm";
import { useToggle } from "../../hooks/useToggle";
import ProtectedLogin from "../../routes/middlewares/ProtectedLogin";
import { authenticateUser } from "../../services/authService";

export interface LoginForm {
  userName: string;
  password: string;
}

const initialForm = {
  userName: "",
  password: "",
};

const validateForm = (form: LoginForm) => {
  const errors: ErrorObject = {};

  if (!form.userName.trim()) {
    errors.userName = "El campo email es requerido";
  }

  if (!form.password.trim()) {
    errors.password = "El campo password es requerido";
  } else if (form.password.length < 5) {
    errors.password = "El password debe tener al menos 6 caracteres";
  }

  return errors;
};

export function Component() {
  const { signIn } = useAuth();
  const { open, toggle } = useToggle();

  const petition = async (form: LoginForm) => {
    const response = await authenticateUser(form);

    if (!response.success) {
      return response;
    }

    const authentication = response.data;

    signIn({
      email: authentication.email,
      token: authentication.token,
      userName: authentication.userName,
      name: authentication.name,
      operations: authentication.operations,
      redirect: false,
      isLoggedIn: true,
      userId: authentication.userId,
    });
    return response;
  };

  const {
    form,
    errors,
    handleChange,
    handleSubmit,
    success,
    loading,
    message,
  } = useForm<LoginForm, any>(initialForm, validateForm, petition);

  return (
    <ProtectedLogin>
      <section className="flex flex-col md:flex-row justify-center items-center w-screen h-screen">
        <div className="flex items-center px-6 md:mx-auto w-full md:max-w-md lg:max-w-lg xl:max-w-xl">
          <Card className="w-full shadow-[0px_20px_20px_10px_#A0AEC0]">
            <CardBody className="p-10">
              {success != null && <Response message={message} type={success} />}
              <div className="flex justify-center">
                <Image
                  isBlurred
                  isZoomed
                  alt="Esi Logo"
                  className=""
                  src={Images.logo}
                  width={240}
                />
              </div>
              <Form onSubmit={handleSubmit} validationErrors={errors}>
                <Input
                  className={"py-4"}
                  errorMessage={errors?.userName}
                  id={"email"}
                  isInvalid={!!errors?.userName}
                  isRequired={true}
                  label={"Nombre de usuario"}
                  name={"userName"}
                  size="lg"
                  type={"text"}
                  value={form.userName}
                  variant="bordered"
                  onChange={handleChange}
                />
                <Input
                  className={"py-4"}
                  endContent={
                    <button
                      className="focus:outline-none"
                      type="button"
                      onClick={toggle}
                    >
                      {open ? (
                        <Icon name="bi bi-eye-slash-fill text-2xl text-dark pointer-events-none" />
                      ) : (
                        <Icon name="bi bi-eye-fill text-2xl text-dark pointer-events-none" />
                      )}
                    </button>
                  }
                  errorMessage={errors?.password}
                  id={"password"}
                  isInvalid={!!errors?.password}
                  isRequired={true}
                  label={"Contraseña"}
                  name={"password"}
                  size="lg"
                  type={open ? "text" : "password"}
                  value={form.password}
                  variant="bordered"
                  onChange={handleChange}
                />
                <Button
                  fullWidth
                  className="py-4 mt-4 font-bold"
                  color="primary"
                  isLoading={loading}
                  radius="md"
                  size="lg"
                  type="submit"
                  variant="shadow"
                >
                  Iniciar Sesión
                </Button>
              </Form>
            </CardBody>
          </Card>
        </div>
      </section>
    </ProtectedLogin>
  );
}

Component.displayName = "LoginPage";
