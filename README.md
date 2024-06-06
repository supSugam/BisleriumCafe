# Bislerium Café Point-of-Sale (POS) Application ☕

This coursework involved developing a custom point-of-sale (POS) application for Bislerium café (University Project) using C# and .NET MAUI Blazor. The main goal was to build an efficient system to facilitate customer orders and sales reporting per the business needs.

### Screenshots & More Details at: [MyPortfolioWebsite/Projects/BisleriumCafe](https://sugamsubedi.com.np/projects/bislerium-cafe) 🚧

## 1. Background and Context 📜

Bislerium café was seeking a tailored POS solution to enhance operations in their growing establishment. Key requirements centered around enabling staff to easily take orders, apply membership benefits, and generate insightful reports. After consultation, it was determined that a C# Blazor application with JSON data persistence would be well-suited.

## 2. Solution Overview 💡

The implemented solution features password-protected access for security, with a separate authorization layer for pricing configuration. Customers can place orders for coffee and add-ins via an intuitive interface. Membership registration is available, with associated free coffee redemption and discount perks. All transactions are saved to support future reporting. Both daily and monthly PDF sales reports can be produced showing revenue totals and top-selling items.

## 3. Technical Implementation ⚙️

The system leverages:

- **C# .NET MAUI Blazor** for cross-platform delivery
- **MudBlazor** framework for responsive UI components
- **JSON** file handling for persistent data storage
- **iText7** for report generation

## 4. Instructions to Run the Project Solution 🚀

### 4.1 Prerequisites

- .NET 8.0 Installed
- Visual Studio 2022 to run the project locally

Things to know before you run the project:

- On Initial Run, admin is automatically initialized with both username and password: “admin”.
- All the data is persisted in the BisleriumCafeWarehouse directory located in Documents in JSON format.
- The generated sales report is also located in BisleriumCafeWarehouse/SalesReports directory.
- Coffee Types, Addins, and Staffs are supposed to be added manually by yourself.

### 4.2 Run Windows App of Bislerium Café

#### Step 1: Navigate to the Directory

After unzipping the development folder, the first step is to navigate to the following directory.

#### Step 2: Install the Certificate

Open the Certificate file.

#### Step 3: Click on Install Certificate

Select Local Machine and click next.

Choose Yes when prompted to trust and choose “Place all” and Trusted Root Certification options.

Click on “Finish” and wait. This indicates that the certificate was installed.

#### Step 4: Install Bislerium Café App

Open the MSIX file, check the “Launch when ready” and click Install. The app should launch successfully.

### 4.3 Running the Project through Visual Studio

Load the Solution (.sln) file in Visual Studio, then Start without Debugging, making sure Windows Machine is selected.
