import { ReactNode } from "react";
import { Navigate } from "react-router";

import { nameRoutes } from "../../config/constants";
import { useAuth } from "../../hooks/useAuth";
import { useErrorsStore } from "../../stores/useErrorsStore";

interface ProtectedPublicProps {
  children: ReactNode;
}

const ProtectedPublic = ({ children }: ProtectedPublicProps) => {
  const { isLoggedIn } = useAuth();
  const { error } = useErrorsStore();

  if (!isLoggedIn) {
    return <Navigate to={nameRoutes.login} />;
  }

  if (error) {
    return <Navigate to={nameRoutes.error} />;
  }

  return children;
};

export default ProtectedPublic;
