import React, { useEffect, useState } from 'react';
import { 
  StyleSheet, Text, View, FlatList, SafeAreaView, 
  ActivityIndicator, StatusBar, TouchableOpacity, 
  Modal, TextInput, Alert, KeyboardAvoidingView, Platform 
} from 'react-native';
import axios from 'axios';

// Tipos
type Product = {
  id: string;
  name: string;
  sku: string;
  price: number;
  stock: number;
  categoryId: string; 
};

export default function App() {
  // ⚠️ REEMPLAZA LOS X CON TU IP QUE YA FUNCIONÓ
  const API_URL = 'http://192.168.0.2:5000/api'; 

  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  
  // Estado para la Edición
  const [modalVisible, setModalVisible] = useState(false);
  const [editingProduct, setEditingProduct] = useState<Product | null>(null);
  
  // Datos del formulario
  const [formName, setFormName] = useState('');
  const [formPrice, setFormPrice] = useState('');
  const [formStock, setFormStock] = useState('');

  // Cargar Productos
  const loadProducts = async () => {
    setLoading(true);
    try {
      const response = await axios.get(`${API_URL}/Products`);
      setProducts(response.data);
    } catch (error) {
      console.error("Error cargando productos:", error);
      Alert.alert("Error", "No se pudo conectar con el servidor.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadProducts();
  }, []);

  // Abrir Modal con datos cargados
  const openEditModal = (product: Product) => {
    setEditingProduct(product);
    setFormName(product.name);
    setFormPrice(product.price.toString());
    setFormStock(product.stock.toString());
    setModalVisible(true);
  };

  // Guardar Cambios (PUT)
  const handleSave = async () => {
    if (!editingProduct) return;

    try {
      // Enviamos el PUT a la API
      await axios.put(`${API_URL}/Products/${editingProduct.id}`, {
        id: editingProduct.id,
        name: formName,
        sku: editingProduct.sku, // Mantenemos el SKU original
        price: parseFloat(formPrice),
        stock: parseInt(formStock),
        categoryId: editingProduct.categoryId // Mantenemos la categoría original
      });

      setModalVisible(false);
      Alert.alert("Éxito", "Producto actualizado correctamente");
      loadProducts(); // Recargar lista automáticamente
    } catch (error) {
      console.error(error);
      Alert.alert("Error", "No se pudo actualizar. Verifica que los datos sean válidos.");
    }
  };

  const renderItem = ({ item }: { item: Product }) => (
    <TouchableOpacity style={styles.card} onPress={() => openEditModal(item)}>
      <View style={styles.header}>
        <Text style={styles.sku}>{item.sku}</Text>
        <Text style={[styles.stockBadge, { color: item.stock < 10 ? 'red' : 'green' }]}>
          {item.stock} u.
        </Text>
      </View>
      <Text style={styles.name}>{item.name}</Text>
      <Text style={styles.price}>
        USD {new Intl.NumberFormat("es-PY").format(item.price)}
      </Text>
      <Text style={styles.hint}>Toca para editar ✏️</Text>
    </TouchableOpacity>
  );

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar barStyle="dark-content" />
      <View style={styles.titleContainer}>
        <Text style={styles.title}>📱 Inventario Móvil</Text>
        <TouchableOpacity onPress={loadProducts} style={styles.refreshButton}>
            <Text style={styles.refreshText}>↻</Text>
        </TouchableOpacity>
      </View>

      {loading ? (
        <ActivityIndicator size="large" color="#0f172a" style={{ marginTop: 50 }} />
      ) : (
        <FlatList
          data={products}
          keyExtractor={(item) => item.id}
          renderItem={renderItem}
          contentContainerStyle={styles.list}
          refreshing={loading}
          onRefresh={loadProducts}
        />
      )}

      {/* MODAL DE EDICIÓN */}
      <Modal
        animationType="slide"
        transparent={true}
        visible={modalVisible}
        onRequestClose={() => setModalVisible(false)}
      >
        <KeyboardAvoidingView 
            behavior={Platform.OS === "ios" ? "padding" : "height"}
            style={styles.centeredView}
        >
          <View style={styles.modalView}>
            <Text style={styles.modalTitle}>Editar Producto</Text>
            
            <Text style={styles.label}>Nombre</Text>
            <TextInput
              style={styles.input}
              value={formName}
              onChangeText={setFormName}
            />

            <Text style={styles.label}>Precio (USD)</Text>
            <TextInput
              style={styles.input}
              value={formPrice}
              onChangeText={setFormPrice}
              keyboardType="numeric"
            />

            <Text style={styles.label}>Stock</Text>
            <TextInput
              style={styles.input}
              value={formStock}
              onChangeText={setFormStock}
              keyboardType="numeric"
            />

            <View style={styles.modalButtons}>
              <TouchableOpacity 
                style={[styles.button, styles.buttonCancel]}
                onPress={() => setModalVisible(false)}
              >
                <Text style={styles.textStyle}>Cancelar</Text>
              </TouchableOpacity>

              <TouchableOpacity 
                style={[styles.button, styles.buttonSave]}
                onPress={handleSave}
              >
                <Text style={styles.textStyle}>Guardar</Text>
              </TouchableOpacity>
            </View>
          </View>
        </KeyboardAvoidingView>
      </Modal>

    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f1f5f9' },
  titleContainer: { 
    padding: 20, backgroundColor: '#fff', borderBottomWidth: 1, borderBottomColor: '#e2e8f0',
    flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center'
  },
  title: { fontSize: 24, fontWeight: 'bold', color: '#0f172a' },
  refreshButton: { padding: 5 },
  refreshText: { fontSize: 24, fontWeight: 'bold' },
  list: { padding: 16 },
  card: {
    backgroundColor: 'white', borderRadius: 12, padding: 16, marginBottom: 12,
    shadowColor: '#000', shadowOpacity: 0.1, shadowRadius: 4, elevation: 3,
  },
  header: { flexDirection: 'row', justifyContent: 'space-between', marginBottom: 8 },
  sku: { fontSize: 12, color: '#64748b', fontWeight: 'bold', backgroundColor: '#f1f5f9', paddingHorizontal: 8, paddingVertical: 2, borderRadius: 4, overflow: 'hidden' },
  stockBadge: { fontWeight: 'bold' },
  name: { fontSize: 18, fontWeight: '600', color: '#334155', marginBottom: 4 },
  price: { fontSize: 16, fontWeight: 'bold', color: '#0f172a' },
  hint: { fontSize: 12, color: '#64748b', marginTop: 10, fontStyle: 'italic', textAlign: 'right' },
  
  // Estilos del Modal
  centeredView: { flex: 1, justifyContent: "center", alignItems: "center", backgroundColor: 'rgba(0,0,0,0.5)' },
  modalView: { width: '85%', backgroundColor: "white", borderRadius: 20, padding: 25, shadowColor: "#000", shadowOffset: { width: 0, height: 2 }, shadowOpacity: 0.25, shadowRadius: 4, elevation: 5 },
  modalTitle: { fontSize: 20, fontWeight: 'bold', marginBottom: 15, textAlign: "center" },
  label: { alignSelf: 'flex-start', marginLeft: 5, marginBottom: 5, fontWeight: '600', color: '#334155' },
  input: { width: '100%', height: 40, borderColor: '#cbd5e1', borderWidth: 1, borderRadius: 8, marginBottom: 15, paddingHorizontal: 10, backgroundColor: '#f8fafc' },
  modalButtons: { flexDirection: 'row', justifyContent: 'space-between', width: '100%', marginTop: 10 },
  button: { borderRadius: 10, padding: 10, elevation: 2, width: '45%' },
  buttonSave: { backgroundColor: "#0f172a" },
  buttonCancel: { backgroundColor: "#ef4444" },
  textStyle: { color: "white", fontWeight: "bold", textAlign: "center" }
});