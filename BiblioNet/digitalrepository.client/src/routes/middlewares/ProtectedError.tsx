import { Navigate } from "react-router";

import { nameRoutes } from "../../config/constants";
import { useAuth } from "../../hooks/useAuth";

interface ProtectedProps {
  children: React.ReactNode;
}

const ProtectedError = ({ children }: ProtectedProps) => {
  const { isLoggedIn, redirect } = useAuth();

  if (!isLoggedIn) {
    return <Navigate to={nameRoutes.login} />;
  }

  if (redirect) {
    return <Navigate to={nameRoutes.changePassword} />;
  }

  return children;
};

export default ProtectedError;
