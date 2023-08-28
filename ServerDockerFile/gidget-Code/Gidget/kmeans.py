from sklearn.cluster import KMeans
from sklearn.cluster import AgglomerativeClustering
import copy
import os
import time
import csv 

def Jsonparse(filename):
    f = open(filename)
    data = f.read()
    pieces = data.split(':')
    failCount = []
    energyUsed = []
    for p in pieces:
        if p.endswith("minEnergy\""):
            p = p.split(',')
            failCount.append(p[0])
        elif p.endswith("endTime\""):
            p = p.split(',')
            energyUsed.append(p[0])
   # print(failCount)
    #print(energyUsed)
    f.close()
    return failCount+energyUsed

def CsvPull(filename):
    X = [] #all level data
    #XX = [] #only levels 1-17

    #get data from source file
    #with open('source.csv','rb') as csvfile:
    with open(filename) as csvfile: 
        reader = csv.reader(csvfile, delimiter=',', quotechar='|') 
        for row in reader:
            #cut off text labels
            X.append(row[:-1])
            #XX.append(row[:-3])

    #cut off title labels
    X = X[1:]
    #XX = XX[1:]
    return X

def MLcalc(X):
    

    #P3 should be classified as low (i.e. best performance)
    #P3 = [[1,0,1,3,2,2,2,8,4,2,0,0,0,2,3,4,5,2,4,6,9,30,30,30,30,30,30,30,30,30,30,30,30,30,30,30]]
    #levels 1-17
    #P3X = [[1,0,1,3,2,2,2,8,4,2,0,0,0,2,3,4,5]]

    #K Means Clustering
    start = time.time()
    kmeans = KMeans(n_clusters=3)
    kmeans.fit(X)
    #print(kmeans.cluster_centers_)
    print(kmeans.labels_)
    KMeansTime = time.time()-start
    print ("K Means Time =", KMeansTime)
    return kmeans.labels_

#f,e = Jsonparse("5d2cbc88654fe.json")
X = []
with os.scandir('results/') as entries:
    for entry in entries:
        X.append(Jsonparse('results/'+entry.name))
        if(len(X[-1])<36):
            X.pop(-1)
'''print (f+e)
X = CsvPull('data.csv')
'''
labels = MLcalc(X)



maxes = [0]*36
mins = [9999]*36
for entry in X:
    #print(entry)
    for i in range(0,36):
        if int(entry[i])>maxes[i]:
            maxes[i]=int(entry[i])
        elif int(entry[i])<mins[i]:
            mins[i]=int(entry[i])
print(maxes)
print(mins)

norms = []
originalData = copy.deepcopy(X)
    
for i in range(0,len(X)):
    for j in range(0,36):
        if mins[j] == maxes[j]:
            X[i][j] = 0.5
        else:
            X[i][j] = (int(X[i][j])-mins[j])/(maxes[j]-mins[j])
            
scores = []
cats = [9999,9999,9999]

for j in range(0,len(X)):
    total=0
    for i in range(0,36):
        total += X[j][i]
    scores.append(total)
    cats[labels[j]]=total if total < cats[labels[j]] else cats[labels[j]]


print(scores)
print(cats)
hi = cats.index(min(cats))
lo = cats.index(max(cats))
med = 3-hi-lo

print(labels)
for i in range(0,len(labels)):
    if (labels[i]==lo):
        labels[i]=2
    elif (labels[i]==med):
        labels[i]=1
    else: 
        labels[i]=0
print (labels)

f=open("values.csv","w")
for i in range(0,len(X)):
    string = ""
    for j in range(0,36):
        string += str(originalData[i][j]) + ','
    string+=str(labels[i])
    f.write(string+'\n')
f.close()
#TODO: Write to CSV
    
#for i in range(0,len(X)):
    
        
    
#print(X)

'''
means = [0]*36
for entry in X:
    #print(entry)
    for i in range(0,36):
        means[i]+=(float(entry[i])/len(X))
print(means)

stds = [0]*36
for entry in X:
    for i in range(0,36):
        stds[i] += (float(entry[i])-means[i])**2
for i in range(0,36):
    stds[i] = (stds[i]/len(X))**(0.5)
print (stds)
        

#for i in range(len(labels)):
#    for entry in X[i]:
        '''
    

#Agglomerative Clustering
'''start = time.time()
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
'''
