import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import type { Product } from "@/types/product";
import { api } from "@/lib/axios";

// 1. Definimos el tipo para Categoría aquí (o en un archivo separado types/category.ts)
type Category = {
  id: string;
  name: string;
};

interface EditProductDialogProps {
  product: Product | null;
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onSuccess: () => void;
}

export function EditProductDialog({ product, open, onOpenChange, onSuccess }: EditProductDialogProps) {
  // Estado del formulario
  const [formData, setFormData] = useState({
    name: "",
    sku: "",
    price: 0,
    categoryId: "" 
  });
  
  // Estado para la lista de categorías
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(false);

  // 2. Cargar Categorías al montar el componente
  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const response = await api.get("/Categories");
        setCategories(response.data);
      } catch (error) {
        console.error("Error cargando categorías:", error);
      }
    };
    fetchCategories();
  }, []);

  // Cargar datos del producto cuando se abre
  useEffect(() => {
    if (product) {
      setFormData({
        name: product.name,
        sku: product.sku,
        price: product.price,
        // OJO: Asegúrate de que tu endpoint GET /products devuelva 'categoryId'. 
        // Si no lo devuelve, tendrás que arreglar el Backend o el Select saldrá vacío.
        categoryId: product.categoryId || "" 
      });
    }
  }, [product]);

  const handleSave = async () => {
    if (!product) return;
    setLoading(true);
    try {
      await api.put(`/Products/${product.id}`, {
        id: product.id,
        ...formData,
        // 3. ¡Ahora sí usamos el ID real seleccionado!
        categoryId: formData.categoryId 
      });
      
      onSuccess(); 
      onOpenChange(false);
      alert("Producto actualizado correctamente");
    } catch (error) {
      console.error(error);
      alert("Error al actualizar. Revisa la consola.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[425px]">
        <DialogHeader>
          <DialogTitle>Editar Producto</DialogTitle>
        </DialogHeader>
        <div className="grid gap-4 py-4">
          
          {/* Nombre */}
          <div className="grid grid-cols-4 items-center gap-4">
            <Label htmlFor="name" className="text-right">Producto</Label>
            <Input
              id="name"
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              className="col-span-3"
            />
          </div>

          {/* SKU */}
          <div className="grid grid-cols-4 items-center gap-4">
            <Label htmlFor="sku" className="text-right">SKU</Label>
            <Input
              id="sku"
              value={formData.sku}
              onChange={(e) => setFormData({ ...formData, sku: e.target.value })}
              className="col-span-3"
            />
          </div>

          {/* Precio */}
          <div className="grid grid-cols-4 items-center gap-4">
            <Label htmlFor="price" className="text-right">Precio</Label>
            <Input
              id="price"
              type="number"
              value={formData.price}
              onChange={(e) => setFormData({ ...formData, price: Number(e.target.value) })}
              className="col-span-3"
            />
          </div>

          {/* 4. Selector de Categoría (Dinámico) */}
          <div className="grid grid-cols-4 items-center gap-4">
            <Label htmlFor="category" className="text-right">Categoría</Label>
            <div className="col-span-3">
                <Select 
                    value={formData.categoryId} 
                    onValueChange={(value) => setFormData({...formData, categoryId: value})}
                >
                <SelectTrigger>
                    <SelectValue placeholder="Seleccionar categoría" />
                </SelectTrigger>
                <SelectContent>
                    {categories.map((cat) => (
                        <SelectItem key={cat.id} value={cat.id}>
                            {cat.name}
                        </SelectItem>
                    ))}
                </SelectContent>
                </Select>
            </div>
          </div>

        </div>
        <DialogFooter>
          <Button onClick={handleSave} disabled={loading}>
            {loading ? "Guardando..." : "Guardar Cambios"}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}