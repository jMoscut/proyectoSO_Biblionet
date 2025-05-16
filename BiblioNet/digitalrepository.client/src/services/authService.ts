import { api } from "../config/axios/interceptors";
import { ChangePasswordForm } from "../pages/auth/ChangePasswordPage";
import { ApiResponse } from "../types/ApiResponse";
import { LoginRequest, LoginResponse } from "../types/LoginRequest";
import { ValidationFailure } from "../types/ValidationFailure";

export const authenticateUser = async (login: LoginRequest) => {
  console.log("login", login);
  const response = await api.post<
    any,
    ApiResponse<LoginResponse>,
    LoginRequest
  >("/auth", login);

  return response;
};

export const changePassword = async (credentials: ChangePasswordForm) => {
  return await api.post<any, ApiResponse<string | ValidationFailure[]>, any>(
    "/Auth/ResetPassword",
    credentials,
  );
};
