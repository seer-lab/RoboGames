from sklearn.cluster import KMeans
from sklearn.cluster import AgglomerativeClustering
import time
import csv


def getCompetence(playerData, filename):
    lData = []  # all level data
    # XX = [] #only levels 1-17
    catData = []  # category data

    # get data from source file
    # with open('source.csv','rb') as csvfile:
    with open(filename) as csvfile:
        reader = csv.reader(csvfile, delimiter=",", quotechar="|")
        for row in reader:
            # cut off text labels
            lData.append(row[:-1])
            catData.append(int(row[-1]))
    # XX.append(row[:-3])

    # XX = XX[1:]
    plen = int(len(playerData) / 2)
    # print(plen)
    # print(lData)
    # print(catData)
    Z = []
    for entry in lData:
        Z.append(entry[:plen] + entry[18 : 18 + plen])

    Z.append(playerData)
    # print(Z)
    kmeans = KMeans(n_clusters=3)
    kmeans.fit(Z)
    solution = [0, 0, 0]
    playerfit = kmeans.labels_[-1]
    for i in range(0, len(kmeans.labels_) - 1):
        if playerfit == kmeans.labels_[i]:
            solution[catData[i]] += 1

    # print(kmeans.labels_)

    if solution[0] > solution[1]:
        if solution[0] > solution[2]:
            return 0
        else:
            return 2
    else:
        if solution[1] > solution[2]:
            return 1
        else:
            return 2


# test code
# print(getCompetence(['0', '2', '0', '0', '1', '2', '2', '1', '1', '5', '1', '8', '3', '1', '1', '13', '5', '22', '1', '5', '44', '15', '43', '88', '35', '97', '181', '108', '43', '63', '101', '38', '43', '178', '147', '158'],'data.csv'))
print(
    getCompetence(
        [
            "1",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "1",
            "0",
            "0",
            "0",
            "2",
            "0",
            "3",
            "0",
            "0",
            "1",
            "0",
            "1",
            "5",
            "37",
            "14",
            "29",
            "79",
            "58",
            "92",
            "156",
            "110",
            "37",
            "53",
            "49",
            "86",
            "53",
            "47",
            "130",
            "127",
        ],
        "data.csv",
    )
)


# Agglomerative Clustering
"""start = time.time()
AgClustering = AgglomerativeClustering(n_clusters=3).fit(X+P3)
print(AgClustering.labels_)
AgClusteringTime = time.time()-start
print ("Agglomerative Clustering Time =", AgClusteringTime)

#K Means Clustering 1-17
start = time.time()
kmeans = KMeans(n_clusters=3,max_iter=300)
kmeans.fit(XX+P3X)
#print(kmeans.cluster_centers_)
print(kmeans.labels_)
KMeansTimeX = time.time()-start
print "K Means Time 1-17 =", KMeansTimeX
"""
