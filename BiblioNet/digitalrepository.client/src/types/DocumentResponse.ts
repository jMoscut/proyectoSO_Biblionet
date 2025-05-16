export interface DocumentResponse {
  id: number;
  userId: number;
  documentNumber: string;
  author: string;
  path: string;
  size: number;
  elaborationDate: string;
  loadDate: string;
  userIp: string;
  state: number;
  createdBy: number;
  updatedBy: number | null;
  createdAt: string;
  updatedAt: string | null;
}
