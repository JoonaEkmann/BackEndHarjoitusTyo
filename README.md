Back-end harjoitustyö, Joona Ekman

Taustaa:
Sovelluksen päätehtävänä on mahdollistaa käyttäjille viestien lähettäminen joko julkisesti tai yksityisesti. Julkiset viestit ovat kaikkien käyttäjien nähtävissä, kun taas yksityiset viestit näkyvät vain niiden lähettäjälle ja vastaanottajalle.
Tässä käyn läpi projektin kansioita ja niiden sisäisiä tehtäviä.

Program.cs:
Tiedostossa määritellään sovelluksen palvelut ja middleware-komponentit, kuten tietokannan yhteys, autentikointi, API-dokumentointi (Swagger) ja viestejä sekä käyttäjiä koskevat palvelut. Se myös huolehtii reitityksistä, HTTPS-ohjauksesta ja API-avaimen tarkistuksesta, sekä varmistaa, että tietokanta on luotu ja sovellus valmis käsittelemään pyyntöjä.

Controllers: 
MessagesController Hallinnoi viestejä sekä tarjoaa toiminnot viestien hakemiseen, luomiseen, päivittämiseen ja poistamiseen.
-GET: Hakee kaikki viestit tai yksittäisen viestin ID:n perusteella.
-POST: Luo uuden viestin.
-PUT: Päivittää viestin.
-DELETE: Poistaa viestin, jos käyttäjä on viestin omistaja.

UsersController: Hallinnoi käyttäjätietoja sekä mahdollistaa käyttäjien hakemisen, luomisen, päivittämisen ja poistamisen.
-GET: Hakee kaikki käyttäjät tai tietyn käyttäjän tunnuksen perusteella.
-POST: Luo uuden käyttäjän.
-PUT: Päivittää käyttäjän tiedot.
-DELETE: Poistaa käyttäjän tunnuksen perusteella.

Middleware:
- ApiKeyMiddleware tarkistaa jokaisen sovelluksen pyynnön API-avaimen oikeellisuuden ennen kuin se päästetään käsittelyyn. Jos avain puuttuu, middleware palauttaa 401 Unauthorized -virheen, ja väärällä avaimella 403 Forbidden. Tämä lisäsuojaus varmistaa, että vain oikeutetut pyynnöt pääsevät API:n käsiksi.

- BasicAuthenticationHandler käsittelee käyttäjän autentikoinnin perustason HTTP-menetelmällä, jossa käyttäjänimi ja salasana haetaan ja tarkistetaan Authorization-otsikosta. Jos autentikointi onnistuu, käyttäjälle annetaan AuthenticationTicket, joka oikeuttaa käyttöoikeuksiin sovelluksessa.

- UserAuthenticationService toteuttaa nämä käytännössä: se tarkistaa käyttäjän oikeellisuuden vertaamalla salasanaa tietokannan hash-arvoon ja varmistaa, että käyttäjä saa nähdä vain omat viestinsä. Yhdessä nämä varmistavat, että vain valtuutetut käyttäjät pääsevät sovelluksen toimintoihin, ja että yksityisviestit ovat suojattuja.

Models:
- Message-luokka edustaa viestejä, joilla on tunniste (Id), otsikko (Title), valinnainen sisältö (Body), lähettäjä (Sender), vastaanottaja (Recipient) ja mahdollinen aiempi viesti (PrevMessage). 
- MessageDTO on viestin siirrettävä versio, joka tallentaa käyttäjätunnukset merkkijonoina ja viittaavat aiempiin viesteihin tunnisteilla. 

- MessageServiceContext on tietokannan konteksti, joka sisältää viestien ja käyttäjien tallentamiseen ja hakemiseen tarvittavat DbSet-kokoelmat. 

- User-luokka tallentaa käyttäjän perustiedot (tunniste, käyttäjänimi, salasana, yhteystiedot, etu- ja sukunimi) sekä aikaleimat rekisteröitymiselle ja viimeiselle kirjautumiselle. 
- UserDTO on kevennetty versio, joka ei sisällä arkaluonteisia tietoja, kuten salasanoja.

Repositories:
- iMessageRepository ja iUserRepository ovat rajapintoja, jotka määrittelevät tietokantatoiminnot viestien ja käyttäjien käsittelemiseksi. 

- MessageRepository toteuttaa iMessageRepository-rajapinnan ja tarjoaa metodit viestien hakemiseen, luomiseen, päivittämiseen ja poistamiseen. 

- UserRepository toteuttaa iUserRepository-rajapinnan ja tarjoaa metodit käyttäjien hakemiseen, luomiseen, päivittämiseen ja poistamiseen.

Services:
- iMessageService ja iUserService ovat palvelurajapintoja, jotka määrittelevät sovelluksen liiketoimintalogiikan viestien ja käyttäjien hallintaan. 

- MessageService toteuttaa iMessageService-rajapinnan ja käyttää MessageRepository- ja iUserRepository-rajapintoja viestien ja käyttäjien hallintaan. Se muuntaa Message- ja MessageDTO-oliot toisikseen ja tarjoaa metodit viestien hakemiseen, luomiseen, päivittämiseen ja poistamiseen. 

- UserService toteuttaa iUserService-rajapinnan ja hallitsee käyttäjätietoja, varmistaen, että käyttäjät voivat kirjautua sisään, luoda uusia tilejä, päivittää tietoja ja poistaa tilejä tarvittaessa.

