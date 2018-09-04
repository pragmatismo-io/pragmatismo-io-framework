Pragmatismo.io Methodology

Copyright (c) Pragmatismo.io. All rights reserved.                          
Licensed under the MIT license                                              

6.2. Dados
----------

### 6.2.1. SQL Server

#### 6.2.1.1. Exportando via BCP

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
BCP "SELECT CONTENT FROM [dbo].[EXPORTAC]" QUERYOUT
C:\Users\username\Desktop\1.xml -T
-fC:\Users\username\Desktop\bcpFormat.fmt -S 127.0.0.1
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

#### 6.2.1.2. Formatação

Use:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
UPDATE ContractProducts

SET
    ContractID = @ContractID,
    ProductID = @ProductID,
    OverdueDate = @OverdueDate,
    OverdueValue = @OverdueValue

WHERE
    (ContractProductID = @ContractProductID)
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Ao invés de:

`UPDATE ContractProducts SET ContractID = @ContractID, ProductID = @ProductID,
OverdueDate = @OverdueDate, OverdueValue = @OverdueValue WHERE
(ContractProductID = @ContractProductID)`

### 6.2.2. Mongo DB

#### 6.2.2.1 Exemplo de uso de geolocalização com Mongo DB

##### Índices de geo-localização no Mongo DB

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ javascript
    loc
    {
        "loc" : 1
    }

    loc_2dsphere
    {
        "loc" : "2dsphere"
    }
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

##### Método de serviço

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ javascript
    find: function(latit, longit, callback) {
        MongoClient.connect(process.env.MONGODB_URL, function(err, db) {
            if (err) throw err;
            var query = {
                loc: {
                    $near: {
                        $geometry: {
                            type: "Point",
                            coordinates: [latit, longit]
                        }
                    }
                }
            };

            db.collection('nomeDaTabela').find(query).toArray(function(err, result) {
                db.close();
                callback(err, result);
            });
        });
    }
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

#### 6.2.3.2 Transação com Sequelize

https://stackoverflow.com/questions/42870374/node-js-7-how-to-use-sequelize-transaction-with-async-await


### 6.2.3. Ciência de Dados

| Title | URL | Description |
|-----|-----|------|
| Visual Object Tagging Tool | https://github.com/Microsoft/VoTT | An electron app for building end to end Object Detection Models from Images and Videos.


### 6.2.3.1. Azure Spark SQL vs U-SQL

https://stackoverflow.com/questions/35575080/azure-spark-sql-vs-u-sql

| Image Similarity using Microsoft CNTK | https://github.com/Azure/ImageSimilarityUsingCntk |
| VoTT: Visual Object Tagging Tool      | https://github.com/Microsoft/VoTT                 |

### 6.2.3.3. Microsoft Machine Learning for Apache Spark

https://github.com/Azure/mmlspark


https://github.com/codefoster/iot-workshop
http://ndres.me/kaggle-past-solutions/
https://www.kaggle.com/


| Título                                  | Endereço                                                    |
|-----------------------------------------|-------------------------------------------------------------|
| Raspberry Pi Azure IoT Online Simulator | https://azure-samples.github.io/raspberry-pi-web-simulator/ |
| Azure IoT Partner Directory             | http://aka.ms/FindAzureIoTPartners                          |
| Azure IoT Certification                 | https://iotcert.cloudapp.net/                               |
| Azure IoT Partners                      | http://azureiotpartners.azurewebsites.net                   |
