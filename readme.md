# HtmlScraperLibrary
The project showcases of my html scraper.

## Overview

HtmlScraperLibrary is a library used to scrape HTML from a config file.
The config file is written manually and is configurable.

## System Requirements

.NET 8

## Libraries and Integrated Tools

Nugets
* HtmlAgilityPack
* Fizzler.Systems.HtmlAgilityPack
* Nito.AsyncEx.Content
* PuppeteerSharp


## Xaml properties

### Root
Needed in a config file even if loaded in local.
* Property [object] -> Preload a property before used.
> \<property name="{name}">{value}\</property>
* Proxy [object] -> Used by WebComponent.
> \<proxy url="http://127.0.0.1:8080"/>

### Scraper
Needed in a config file even if loaded in local.
* Name [string] -> Used to difference between each scraper, not necessary.
* Blacklist [object] -> Used to delete datas not desired to be scraped. If variable with this value found.
> \<blacklist variable="{variable}" value="{variable}"/>

### Web
Needed in a config file even if loaded in local.
* Url [string] -> Url to visit
* Chrome [bool] -> Open an HTTPClient or a chrome window
* Headers [object] -> Preloaded headers
> \<cookie name="{name}" value="{value}"/>
* Cookies [object] -> Preloaded cokiees
> \<cookie name="{name}" value="{value}" path="{path}" domain="{domain}"/>

### Select
* Query [string] -> Html query.
> query=".class #id div"
* To [string] -> Key to stock the json data.
* Single [bool] -> Means there is only one value to get here, if not it will create a json array.

### List
Needed in a config file even if loaded in local.
* Loop [string] -> Tell to the scraper the number of loop to domain
> \<list loop="page:{pageStart}:{pageEnd}">


### Text
Get value from inner text
* Regex [string] -> Parse the value.
* RegexGroup [int] -> Get the parsed group.
* Trim [bool] -> Trim the start and end of the data.
* HtmlDecode [bool] -> Change Html codes to plain text.
* To [string] -> Key to stock the json data.
* Property [string] -> Set the property with the value.
* Empty [string] -> Insert a value if empty
* Replace [object] -> Replace value to another
> \<replace pattern="{regex}" by="{value}"/>
* Before [object] -> Add data before the raw data.
> \<before value="{value}"/>

### Value
Set a raw value into the json result.
* Value [string] -> Set the raw data.
* Regex [string] -> Parse the value.
* RegexGroup [int] -> Get the parsed group.
* Trim [bool] -> Trim the start and end of the data.
* HtmlDecode [bool] -> Change Html codes to plain text.
* To [string] -> Key to stock the json data.
* Property [string] -> Set the property with the value.
* Empty [string] -> Insert a value if empty
* Replace [object] -> Replace value to another
> \<replace pattern="{regex}" by="{value}"/>

### Attribute
Get value from the attribute
* Attribute [string] -> Attribute key.
> attribute="href"
* Regex [string] -> Parse the value.
* RegexGroup [int] -> Get the parsed group.
* Trim [bool] -> Trim the start and end of the data.
* HtmlDecode [bool] -> Change Html codes to plain text.
* To [string] -> Key to stock the json data.
* Property [string] -> Set the property with the value.
* Empty [string] -> Insert a value if empty
* Replace [object] -> Replace value to another
> \<replace pattern="{regex}" by="{value}"/>
* Before [object] -> Add data before the raw data.
> \<before value="{value}"/>


### BlackList
Blacklist matching datas
* Variable [string] -> Property to check as deletion
* Value [string] -> Value to check as deletion

### Replace
Set to replace values into ComponenentAttribute, ComponenentText, ComponenentValue.
* Pattern [string] -> Set a Regex
* By [string] -> Pattern replaced by this value