import "bootstrap-icons/font/bootstrap-icons.css";
import { createRoot } from "react-dom/client";
import App from "./routes/App.tsx";
import "./styles/styles.css";

createRoot(document.getElementById("root")!).render(<App />);
