# Sentiment_Analysis_Supervised_ML

8 movie review sites = 4 Training Dataset + 4 Test Dataset.

Web App crawled (fixed depth) the training dataset, & for each movie, its name, details, review & user comments were scraped out using Html Agility Pack’s DOM based Library. These were tokenized & matched against a DB of words (adjectives majorly) to produce scores (+ve, -ve & neutral). Score obtained after scraping test dataset & scores calculated previously using training data set were averaged to get final rating out of 5.
