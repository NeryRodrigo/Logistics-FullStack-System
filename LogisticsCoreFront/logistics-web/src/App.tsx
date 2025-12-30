import { useEffect, useState } from "react";
import { DataTable } from "@/components/ui/data-table";
import type { Product } from "@/types/product";
import { api } from "@/lib/axios";
import { Button } from "@/components/ui/button";
import { type ColumnDef } from "@tanstack/react-table";
import { Edit, Trash2 } from "lucide-react"; // Iconos
import { EditProductDialog } from "@/features/products/EditProductDialog";

function App() {
  const [data, setData] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  
  // Estado para la edición
  const [editingProduct, setEditingProduct] = useState<Product | null>(null);
  const [isEditOpen, setIsEditOpen] = useState(false);

  const loadProducts = async () => {
    try {
      const response = await api.get("/Products");
      setData(response.data);
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadProducts();
  }, []);

  // Definimos las columnas aquí para poder usar las funciones de editar
  const columns: ColumnDef<Product>[] = [
    { accessorKey: "sku", header: "SKU" },
    { accessorKey: "name", header: "Producto" },
    { 
      accessorKey: "price", 
      header: "Precio",
      cell: ({ row }) => new Intl.NumberFormat("es-PY", { style: "currency", currency: "USD" }).format(row.getValue("price"))
    },
    { 
        accessorKey: "stock", 
        header: "Stock",
        cell: ({ row }) => (
            <span className={row.getValue<number>("stock") < 10 ? "text-red-500 font-bold" : "text-green-600 font-bold"}>
                {row.getValue("stock")} u.
            </span>
        )
    },
    {
      id: "actions",
      cell: ({ row }) => {
        const product = row.original;
        return (
          <div className="flex gap-2">
            <Button variant="outline" size="icon" onClick={() => {
                setEditingProduct(product);
                setIsEditOpen(true);
            }}>
              <Edit className="h-4 w-4" />
            </Button>
            <Button variant="destructive" size="icon">
               <Trash2 className="h-4 w-4" />
            </Button>
          </div>
        )
      }
    }
  ];

  return (
    <div className="min-h-screen bg-slate-50 p-8 font-sans text-slate-900">
      <div className="mx-auto max-w-5xl space-y-6">
        <div className="flex items-center justify-between">
            <h1 className="text-3xl font-bold tracking-tight">Inventario</h1>
            <Button onClick={loadProducts}>Refrescar</Button>
        </div>

        <div className="bg-white p-4 rounded-xl border shadow-sm">
          {loading ? <div>Cargando...</div> : <DataTable columns={columns} data={data} />}
        </div>

        {/* Modal de Edición */}
        <EditProductDialog 
            open={isEditOpen} 
            onOpenChange={setIsEditOpen} 
            product={editingProduct} 
            onSuccess={loadProducts}
        />
      </div>
    </div>
  );
}

export default App;