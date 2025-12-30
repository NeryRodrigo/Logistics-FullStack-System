import type { ColumnDef } from "@tanstack/react-table"
import type { Product } from "@/types/product"

export const columns: ColumnDef<Product>[] = [
  {
    accessorKey: "sku",
    header: "SKU",
  },
  {
    accessorKey: "name",
    header: "Producto",
  },
  {
    accessorKey: "price",
    header: "Precio",
    cell: ({ row }) => {
      const amount = parseFloat(row.getValue("price"))
      const formatted = new Intl.NumberFormat("es-PY", {
        style: "currency",
        currency: "PYG",
      }).format(amount)
      return <div className="font-medium">{formatted}</div>
    },
  },
  {
    accessorKey: "stock",
    header: "Stock",
    cell: ({ row }) => {
        const stock = parseFloat(row.getValue("stock"))
        return (
            <div className={`font-bold ${stock < 10 ? "text-red-500" : "text-green-600"}`}>
                {stock} u.
            </div>
        )
    }
  },
]