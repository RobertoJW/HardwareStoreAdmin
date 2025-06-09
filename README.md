# 🛒 Hardware Store

**Hardware Store** es una aplicación multiplataforma desarrollada como Trabajo de Fin de Grado del Ciclo Formativo Grado Superior en Desarrollo de Aplicaciones Multiplataforma por Daniel Pajarón y Roberto Jiang. Su objetivo es simular una tienda online de productos informáticos, como móviles, portátiles y ordenadores de sobremesa.

La aplicación ha sido desarrollada con .NET MAUI, permitiendo su uso tanto en dispositivos móviles (Android, iOS) como en ordenadores (Windows, macOS).

## 📱 Funcionalidades principales

- Registro e inicio de sesión de usuarios
- Búsqueda de productos con filtros
- Gestión de productos favoritos
- Carrito de compra
- Simulación de compra (sin sistema de pago real)
- Interfaz adaptable a distintos tamaños de pantalla

## ⚙️ Tecnologías utilizadas

- **Lenguaje principal:** C#
- **Framework multiplataforma:** .NET MAUI
- **Diseño de interfaz:** XAML
- **Gestión de base de datos:** Entity Framework Core
- **Base de datos en la nube:** Google Cloud MySQL
- **API propia en C# (.NET):** para gestionar peticiones entre la app y la base de datos
- **IDE:** Visual Studio 2022

## 🔌 Conexión a la base de datos

La aplicación utiliza Google Cloud MySQL para almacenar:
- Información de usuarios
- Productos
- Carritos de compra
- Favoritos

Se ha utilizado **Entity Framework Core** como ORM para gestionar las operaciones sobre la base de datos de forma sencilla y segura.

## 🌐 API REST

La aplicación se conecta a una **API REST desarrollada en C#** utilizando .NET para manejar toda la comunicación entre la app y la base de datos. Esta API se encarga de:

- Registrar y autenticar usuarios
- Consultar productos
- Añadir o quitar productos del carrito o de la lista de favoritos
- Guardar y recuperar datos desde la base de datos
