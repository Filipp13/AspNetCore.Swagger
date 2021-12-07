# Свагер

Свагер конфигурируется с помощью appsettings.json, пример:

```json
"SwaggerGenerator": {
			"SwaggerDocs": {
			  "v1": {
				"Title": "Employee.Api",
				"Description": "Сервис работников",
				"TermsOfService": "https://cis-confl.deloitteresources.com/display/CPT/PMP",
				"Contact": {
				  "Name": "PMP Team",
				  "Email": "fantipov@deloitte.ru",
				  "Url": "https://cis-confl.deloitteresources.com/display/CPT/PMP"
				}
			  }
			}
		  },
		  "SwaggerUI": {
			"RoutePrefix": "api/employee/swagger",
			"ConfigObject": {
			  "Urls": [
				{
				  "Url": "/api/employee/swagger/v1/swagger.json",
				  "Name": "Employee.API"
				}
			  ]
			}
		  },
		  "Swagger": {
			"RouteTemplate": "api/employee/swagger/{documentname}/swagger.json"
		  }
		}
```

Можно указать ссылки на документацию и прочую информацию. 
Поддерживается расширенная документация из комментариев для чего необходимо добавить в проект их генерацию, пример

```xml
<PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Release|AnyCPU'">
		<DocumentationFile>bin\Debug\net5.0\xxx.Api.xml</DocumentationFile>
	</PropertyGroup>
```