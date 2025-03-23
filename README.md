
### Lista de tehnologii

- **Front-end**:  
  - Framework: React / Vue / Angular (în curs de alegere, React fiind un candidat principal).  
  - Stilizare: Bootstrap 

- **Back-end**:  
  - **C# .NET (ASP.NET Core)** și/sau **TypeScript**
  - **OCR**: Integrare cu Tesseract sau servicii cloud (Azure Cognitive Services, Google Vision).  
  - **Traducere automată**: API terț (Google Translate, Azure Translate).  
  - **Conversie documente**:  
    - Librării: LibreOffice, img2pdf erc.
  - **Editare PDF**:  
    - iText 7 (C#) sau pdf-lib / PDFKit (TypeScript) pentru operațiuni de bază (anotare, semnătură, extragere text).

- **Bază de date**:  
  - SQL (Microsoft SQL Server, PostgreSQL) sau NoSQL (MongoDB), în funcție de complexitatea necesară (versiuni, stocare etc.).

- **Alte considerente**:  
  - **Autentificare**: JWT sau Identity Server (în ASP.NET Core).  
  - **Stocare fișiere**: Azure Blob, AWS S3, Google Cloud Storage.  

---

### Requirments document

| Funcționalitate                                          | Impact | Dificultate | Observații                                                                                                                                       |
|----------------------------------------------------------|--------|-------------|---------------------------------------------------------------------------------------------------------------------------------------------------|
| **Editor PDF**                                           | ⭐⭐⭐  | ⭐⭐       | Permite editarea textului, adăugarea de adnotări, reorganizarea paginilor. Necesită o bibliotecă sau API robustă de lucru cu PDF-uri.            |
| **OCR (Recunoaștere Optică a Caracterelor)**             | ⭐⭐⭐  | ⭐⭐⭐       | Foarte utilă pentru documente scanate sau imagini. Integrare cu Tesseract sau servicii cloud (Azure, Google Vision).                             |
| **Traducere automată**                                   | ⭐⭐⭐  | ⭐⭐       | Esențială pentru traducătorii care doresc să accelereze fluxul. Conectare la un API (Google Translate, Azure Translate).                         |
| **Vizualizare comparativă (Side by Side)**               | ⭐⭐  | ⭐      | Permite compararea rapidă a două documente sau a versiunii inițiale vs. cea editată.                                                             |
| **Conversie documente (imagine <-> PDF <-> DOCX)**          | ⭐⭐⭐  | ⭐⭐       | Funcție de bază pentru lucrul cu formate multiple.                         |
| **Redactare / Cenzurare (Redaction)**                    | ⭐⭐  | ⭐⭐       | Elimină definitiv datele sensibile dintr-un document.                                                |
| **Colaborare (multi-utilizator)**                        | ⭐⭐⭐  | ⭐⭐⭐       | Editare în timp real și gestionarea conflictelor.                                                              |
| **Control versiuni / Track Changes**                     | ⭐⭐⭐  | ⭐⭐⭐       | Urmărește și permite revenirea la diferite versiuni.                                              |
| **Adnotări / Markup**                                    | ⭐⭐  | ⭐⭐       | Comentarii, evidențieri, forme grafice.                                                             |
| **Semnătură electronică / Digitală**                     | ⭐⭐  | ⭐⭐       | Necesită criptografie sau integrare cu servicii de e-sign (DocuSign, Adobe Sign).                                                               |
| **Căutare avansată (Full-text, cuvânt-cheie)**           | ⭐⭐⭐  | ⭐⭐       | Facilitează lucrul cu documente mari.                                         |
| **Autentificare / Autorizare utilizatori**               | ⭐⭐⭐  | ⭐⭐       | Critică pentru securitatea documentelor; gestionare roluri (admin, translator, user simplu).                                                    |
| **Stocare în Cloud / Sincronizare**                      | ⭐⭐⭐  | ⭐⭐       | Permite accesul la documente de pe diferite dispozitive.                                                              |
| **Export adnotări / comentarii**                         | ⭐⭐  | ⭐      | Posibilitatea de a partaja feedback-ul sau notițele într-un format separat sau integrat.                                                         |
| **Memorie de traducere (Translation Memory)**            | ⭐⭐⭐  | ⭐⭐⭐       | Foarte utilă pentru traducătorii profesioniști; stochează segmentele și traducerile lor.                                                        |
| **Glosar / Terminologie specializată**                   | ⭐⭐⭐  | ⭐⭐       | Asigură consecvența pentru termeni specializați și viteză sporită în proiectele de traducere.                                                   |


#### Cerințe nefuncționale

1. **Performanță**  
   - Conversiile să dureze un timp rezonabil.  
2. **Fiabilitate și disponibilitate**  
   - Gestionarea întreruperilor fără coruperea datelor.  
3. **Scalabilitate**  
   - Suport pentru documente mari și utilizatori multipli.  
4. **Securitate**  
   - Criptare (HTTPS) și eventual criptarea la stocare (cloud).  
5. **Utilizabilitate**  
   - Interfață intuitivă, pași simpli pentru operațiile principale.

---

### User Stories

1. **Ca utilizator simplu**:  
   „Doresc să încarc un fișier PDF și să-l convertesc în DOCX pentru a-l edita ulterior.”

2. **Ca utilizator simplu**:  
   „Vreau să aplic OCR pe un fișier needitabil pentru a putea căuta și edita conținutul.”

3. **Ca utilizator**:  
   „Am nevoie să ascund informații sensibile dintr-un PDF prin redactare.”

4. **Ca traducător profesionist**:  
   „Doresc să folosesc traducerea automată pentru a majoritatea documentului, apoi să retușez manual textul.”

5. **Ca traducător profesionist**:  
   „Vreau să-mi construiesc o memorie de traducere și un glosar, pentru a asigura consistența și rapiditatea în proiectele viitoare.”

6. **Ca utilizator**:  
   „Vreau să compar rapid două versiuni ale aceluiași document, ca să văd ce s-a schimbat.”

7. **Ca admin**:  
   „Am nevoie să gestionez conturile de utilizatori, documentele și permisiunile pentru a menține politica de securitate.”
---

### 2.4. Diagrama de Use Case

- **Actori**:  
  1. **Utilizator** (standard, traducător)  
  2. **Administrator**
```mermaid
%%{init: {'theme': 'forest', "flowchart" : { "curve" : "basis" } } }%%

flowchart LR
    subgraph A1[Actori - Utilizatori]
    A[Utilizator]:::actor
    T[Traducător]:::actor
    end
    subgraph UC[Use Cases]
    UC1(Autentificare)
    UC2(Încărcare Document)
    UC3(Editare / Anotare Document)
    UC4(Conversie Document)
    UC5(OCR)
    UC6(Redactare Informații)
    UC7(Traducere Automată)
    UC8(Vizualizare comparativă)
    UC9(Gestionare conturi și permisiuni)
    UC10(Administrare Memorie de traducere / Glosar)
    end
    subgraph A2[Actor - Admin]
    B[Admin]:::actor
    end
    A --> UC1
    A --> UC2
    A --> UC3
    A --> UC4
    A --> UC5
    A --> UC6
    A --> UC8
    T --> UC1
    T --> UC2
    T --> UC3
    T --> UC4
    T --> UC5
    T --> UC6
    T --> UC7
    T --> UC8
    T --> UC10
    B --> UC1
    B --> UC9
    classDef actor fill:#e1f5fe,stroke:#0288d1,stroke-width:1px,color:#0288d1

```
---

### 2.5. Diagrama ER

```mermaid
erDiagram
    ANNOTATION {
        integer annotation_id 
        character_varying annotation_text 
        integer document_id 
        integer page_number 
        integer user_id 
    }

    DOCUMENT {
        integer document_id 
        character_varying original_format 
        character_varying title 
        time_without_time_zone upload_date 
        integer user_id 
    }

    DOCUMENT_FORMAT {
        bytea bytes 
        character_varying checksum 
        time_without_time_zone created_at 
        integer document_id 
        integer format_id 
        character_varying format_type 
    }

    DOCUMENT_VERSION {
        character_varying changes_description 
        time_without_time_zone created_at 
        integer document_id 
        integer version_id 
        integer version_number 
    }

    GLOSSARY_TERMS {
        character_varying domain 
        integer term_id 
        character_varying term_source 
        character_varying term_target 
        integer user_id 
    }

    OPERATION {
        integer document_id 
        character_varying error_message 
        integer operation_id 
        character_varying operation_type 
        character_varying source_format 
        character_varying status 
        character_varying target_format 
        integer user_id 
    }

    TRANSLATION_MEMORY {
        character_varying source_lang 
        character_varying source_text 
        character_varying target_lang 
        character_varying target_text 
        integer tm_id 
        integer user_id 
    }

    USERS {
        character_varying email 
        character_varying password_hash 
        character_varying role 
        integer user_id 
        character_varying username 
    }

    ANNOTATION }o--|| DOCUMENT : ""
    ANNOTATION }o--|| USERS : ""
    DOCUMENT }o--|| USERS : ""
    DOCUMENT_FORMAT }o--|| DOCUMENT : ""
    DOCUMENT_VERSION }o--|| DOCUMENT : ""
    OPERATION }o--|| DOCUMENT : ""
    GLOSSARY_TERMS }o--|| USERS : ""
    OPERATION }o--|| USERS : ""
    TRANSLATION_MEMORY }o--|| USERS : ""
```

---
