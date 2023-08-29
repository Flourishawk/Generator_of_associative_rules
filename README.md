<p align="center">
      <img src="https://i.ibb.co/5LxFJvF/Logo.png" width="325" alt="Logo">
</p>

<p align="center">
   <img src="https://img.shields.io/badge/Technology-WPF%20-blueviolet" alt="Technology">
   <img src="https://img.shields.io/badge/Version-1.0%20(Alpha)-blue" alt="Version">
</p>

## About Assorule

This application allows you to generate association rules from .csv data using Apriori and FP-Growth algorithms.

## Documentation

### Input Data

  Assorule works with transaction-type data, i.e., where the transaction id and product id are specified.
  The application supports both sparse data (the absence of products is indicated by 0) and regular data (the absence of a product is simply not recorded in any way). It is possible to work with a MySQL database if you have MySQLServer 8.0 installed and running accordingly. The application will simply convert the database to .csv data and process it in the usual way, so I recommend using .csv files for faster application operation.
  
#### The .csv format imposes its own syntactic restrictions, namely:
    - the first line is a data header;
    - transactions, in turn, can be formalized in two ways, for example
   <p align="center">
   <img src="https://i.ibb.co/xFpNRbY/Initial.png" alt="Initial Data" align="center" width=300>
   </p>
   <p align="center">
   <img src="https://i.ibb.co/VQw5jNn/Regular.png" alt="Regular Data" align="center" width=300>
   </p>
  Separator symbols and array boundary symbols can be selected in the course of the program or removed altogether.
  
### Export Data

  The statistics of the application performance can be evaluated with the information provided at the end (number of steps, running time and number of rules). The associative rules can be found in the WpfApp1/Export folder in .csv format
