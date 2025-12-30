export type Product = {
  id: string;
  name: string;
  sku: string;
  price: number;
  stock: number;
  categoryId?: string;
  categoryName?: string; // Opcional por ahora hasta que conectemos el include
}