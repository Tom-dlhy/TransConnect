# 🚛 TransConnect

**TransConnect** is a management application for a transportation company, developed in C#. It allows the management of employees, clients, vehicles, and orders while providing advanced features for organizational chart display and route calculation.

## 📋 **Project Overview**

The TransConnect application is designed to simplify the management of a transportation company by providing the following modules:

- **Employee Management**: Add, remove, update, and display employees with complete details.
- **Client Management**: Track clients, add and update their information, sort by alphabetical order, city, and total purchases.
- **Vehicle Management**: Monitor the availability and usage of the fleet.
- **Order Management**: Create, modify, and track transportation orders.
- **Organizational Chart**: Hierarchical visualization using an n-ary tree structure.
- **Statistics**: Generate reports on deliveries, orders, and driver performance.

## 🚀 **Main Features**

- **Client Module:**
  - Add, delete, and update clients from the console or a CSV file.
  - Display clients sorted by name, city, or cumulative purchase amount.

- **Employee Module:**
  - Manage employees with an organizational chart display.
  - Add and remove employees, including integration into the company hierarchy.
  - Display the n-ary tree representing the company's hierarchy.

- **Order Module:**
  - Create orders with client selection, driver, and vehicle assignment.
  - Automatically calculate prices based on distance and driver rates.
  - Track and store delivery history for each client.

- **Vehicle Module:**
  - Manage the fleet of vehicles: cars, vans, heavy trucks.
  - Monitor vehicle usage and check availability.

- **Statistics and Reports:**
  - Analyze deliveries per driver.
  - Generate reports on orders for a given period.
  - Calculate the average price of orders and client account balances.

## 🛠️ **Technologies Used**

- **Language**: C#
- **Paradigm**: Object-Oriented Programming (OOP) with inheritance, abstract classes, and interfaces.
- **Generic Collections**: Use of `List<T>`, `Dictionary<TKey, TValue>`, and implementation of an n-ary tree for the organizational chart.
- **Algorithms**: Implementation of Dijkstra's algorithm for optimized route calculation.

## 📂 **Project Structure**

```plaintext
TransConnect/
├── Program.cs                  # Entry point of the application
├── Entreprise.cs               # Main management class
├── Salarié.cs                  # Class representing an employee
├── Client.cs                   # Class representing a client
├── Commande.cs                 # Class representing an order
├── Véhicule.cs                 # Class representing a vehicle
├── Ville.cs                    # Class representing a city
├── Noeud.cs                    # Class representing a node for the organizational chart
├── Interface.cs                # Interface used in the project
├── Chef_equipe.cs              # Class representing a team leader
├── Chauffeur.cs                # Class representing a driver
└── Problème_Version_1.pdf      # Project specifications and requirements
