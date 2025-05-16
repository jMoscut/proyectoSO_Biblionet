import { ReactNode } from "react";

import DrawerMenu from "../components/layout/DrawerMenu";
import { Footer } from "../components/layout/Footer";
import { NavbarApp } from "../components/layout/NavbarApp";
import { useToggle } from "../hooks/useToggle";

interface LayoutProps {
  children: ReactNode;
}

export const Layout = ({ children }: LayoutProps) => {
  const { open, toggle } = useToggle();

  return (
    <div className="h-screen">
      <NavbarApp toggleDrawer={toggle} />
      <div className="">
        <main className="min-h-[80%] md:mt-0 md:min-h-[95%]">{children}</main>
        <DrawerMenu isOpen={open} onOpenChange={toggle} />
        <Footer />
      </div>
    </div>
  );
};
