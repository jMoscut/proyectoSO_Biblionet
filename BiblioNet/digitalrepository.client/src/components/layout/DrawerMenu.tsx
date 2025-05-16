import { Button } from "@heroui/button";
import {
  Drawer,
  DrawerBody,
  DrawerContent,
  DrawerFooter,
  DrawerHeader,
} from "@heroui/drawer";
import { Link } from "react-router";
import { useAuth } from "../../hooks/useAuth";

export interface DrawerMenuProps {
  isOpen: boolean;
  onOpenChange: (isOpen: boolean) => void;
}

const DrawerMenu = ({ isOpen, onOpenChange }: DrawerMenuProps) => {
  const { allOperations } = useAuth();

  return (
    <Drawer
      backdrop="blur"
      isOpen={isOpen}
      placement="left"
      onOpenChange={onOpenChange}
    >
      <DrawerContent>
        {(onClose) => (
          <>
            <DrawerHeader className="flex flex-col gap-1">
              Repositorio Digital
            </DrawerHeader>
            <DrawerBody>
              <div className="flex flex-col gap-2">
                {allOperations.map((operation) => (
                  <Button
                    as={Link}
                    key={operation.name}
                    to={operation.path}
                    color="primary"
                    variant="flat"
                    onPress={() => {
                      onClose();
                    }}
                  >
                    {operation.name}
                  </Button>
                ))}
              </div>
            </DrawerBody>
            <DrawerFooter>
              <Button color="danger" variant="light" onPress={onClose}>
                Close
              </Button>
            </DrawerFooter>
          </>
        )}
      </DrawerContent>
    </Drawer>
  );
};

export default DrawerMenu;
