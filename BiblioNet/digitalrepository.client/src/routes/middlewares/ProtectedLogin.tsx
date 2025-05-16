import { Navigate } from "react-router";

import { nameRoutes } from "../../config/constants";
import { useAuth } from "../../hooks/useAuth";

interface ProtectedLoginProps {
  children: React.ReactNode;
}

const ProtectedLogin = ({ children }: ProtectedLoginProps) => {
  const { isLoggedIn } = useAuth();

  if (isLoggedIn) {
    return <Navigate to={nameRoutes.root} />;
  }

  return children;
};

export default ProtectedLogin;
