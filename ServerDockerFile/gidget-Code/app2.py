from sklearn.cluster import KMeans
from sklearn.cluster import AgglomerativeClustering
import time, json, csv, uuid,os
import quart
from quart import Quart, websocket, request, jsonify, redirect, flash
from quart_cors import cors;
from zipfile import ZipFile
from werkzeug.utils import secure_filename

static_file_dir = os.path.join(os.path.dirname(os.path.realpath(__file__)), "Gidget")
app = Quart(__name__)
app = cors(app, allow_origin="*")

ALLOWED_EXTENSIONS = set(['csv', 'js'])
def allowed_file(filename):
    return '.' in filename and \
           filename.rsplit('.', 1)[1].lower() in ALLOWED_EXTENSIONS

@app.route("/", methods=["GET"])
def serve_dir_directory_index():
    return quart.send_from_directory(static_file_dir, "index.html")

@app.route("/<path:path>", methods=["GET"])
def serve_file_in_dir(path):

    if not os.path.isfile(os.path.join(static_file_dir, path)):
        path = os.path.join(path, "index.html")

    return quart.send_from_directory(static_file_dir, path)

@app.route("/runKmean", methods=["GET"])
def run_kmean():
    process = os.popen('cd Gidget && python kmeans.py')
    outputs = process.read()
    process.close()
    return quart.Response(outputs)

# @app.route("/switchFlip", methods=["GET"])
# def switch_the_flip():
    

@app.route("/uploadCSVandStuff", methods=["GET", "POST"])
async def serve_file_in_dir_csv():
    if(request.method == 'POST'):
        files = await request.files
        if('file' not in files):
            flash('No File part')
            return redirect(request.url)
        
        file = files['file']
        if(file.filename == ''):
            flash("No selected file")
            return redirect(request.url)

        print(allowed_file(file.filename))
        if file and allowed_file(file.filename):
            filename = secure_filename(file.filename)
            print(filename)
            file.save(os.path.join(static_file_dir, filename))
            return quart.Response("Success")
    return '''
    <!doctype html>
    <title>Upload new File</title>
    <h1>Upload new File</h1>
    <form method=post enctype=multipart/form-data>
      <input type=file name=file>
      <input type=submit value=Upload>
    </form>
    '''

@app.route("/getAllData", methods=["GET"])
def getData():
    with ZipFile('GidgetData.zip','w') as zipObj:
        for foldername,subfolders, filenames in os.walk("Gidget/results/"):
            for filename in filenames:
                filePath = os.path.join(foldername,filename)
                zipObj.write(filePath, filePath[len("Gidget/results/") :])

    #return quart.send_from_directory(static_file_dir, 'GidgetData.zip')
    return quart.send_file('GidgetData.zip')

@app.route("/deleteAllData", methods=["DELETE"])
async def deleteAllData():
    data = await request.get_data()
    data2 = json.loads(data)
    if(not(data2['passwordO'] == "RWnVD3kHTHRvRa" and
        data2['passwordT'] == "RdD3!yC9Ytd6o6oGg3R*5siw2fnLSa#qq!%YEH4zgGo#xeaBdM%QFz4")):
        return quart.Response("Bad Credential")

    for foldername,subfolders, filenames in os.walk("Gidget/results/"):
        for filename in filenames:
            filePath = os.path.join(foldername,filename)
            print(filePath)
            try:
                if os.path.isfile(filePath):
                    os.remove(filePath)
            except Exception as e:
                print(e)
    
    return quart.Response("data has been deleted")

@app.route("/saveData", methods=["POST"])
async def postJsonHandler():
	data = await request.get_data()
	id = uuid.uuid1()
 	#try:
	data2 = json.loads(data)
  	#except:
      	#return "Could not complete request", 404
	print("JsonData: " + str(data2))

	f = open("Gidget/results/" + str(id) + ".json", "w+")
	f.write(str(data2))
	f.close()
	return  quart.abort(200)

@app.route("/api/getCompetence", methods=["GET"])
def getCompetence():

    if "playerData" in request.args:
        playerData = (request.args["playerData"])
    else:
        return "Error: No playerData field provided"
    if "fileName" in request.args:
        fileName = request.args["fileName"]
    else:
        return "Error: No fileName field provided."
    lData = []  # all level data
    # XX = [] #only levels 1-17
    catData = []  # category data

    playerData = playerData.replace('[', '')
    playerData = playerData.replace(']', '')

    num = playerData.split(',')
    for n in range(len(num)):
        num[n] = num[n].replace('\'', '')

    # get data from source file
    # with open('source.csv','rb') as csvfile:
    with open("Gidget/" + fileName) as csvfile:
        # with open(fileName) as csvfile:
        reader = csv.reader(csvfile, delimiter=",", quotechar="|")
        for row in reader:  # cut off text labels
            lData.append(row[:-1])
            catData.append(int(row[-1]))
        # XX.append(row[:-3])

    # XX = XX[1:]
    plen = int((len(num)) / 2)
    print(plen)
    print(lData)
    print(catData)
    print(num)
    Z = []
    for entry in lData:
        Z.append(entry[:plen] + entry[18 : 18 + plen])

    Z.append(num)
    print(Z)
    kmeans = KMeans(n_clusters=3)
    kmeans.fit(Z)
    solution = [0, 0, 0]
    playerfit = kmeans.labels_[-1]
    for i in range(0, len(kmeans.labels_) - 1):
        if playerfit == kmeans.labels_[i]:
            solution[catData[i]] += 1

    print(kmeans.labels_)
    response = ""

    if solution[0] > solution[1]:
        if solution[0] > solution[2]:
            response=  quart.jsonify({"solution": 0})
        else:
            response=  quart.jsonify({"solution": 2})
    else:
        if solution[1] > solution[2]:
            response=  quart.jsonify({"solution": 1})
        else:
            response=  quart.jsonify({"solution": 2})

    
    response.headers.add('Access-Control-Allow-Origin', '*')
    return response


app.run("0.0.0.0",port=8082)
# test code
#print(getCompetence(['0', '2', '0', '0', '1', '2', '2', '1', '1', '5', '1', '8', '3', '1', '1', '13', '5', '22', '1', '5', '44', '15', '43', '88', '35', '97', '181', '108', '43', '63', '101', '38', '43', '178', '147', '158'],'data.csv'))
# print(getCompetence(['1', '0', '0', '0', '0', '0', '0', '1', '0', '0', '0', '2', '0', '3', '0', '0', '1', '0', '1', '5', '37', '14', '29', '79', '58', '92', '156', '110', '37', '53', '49', '86', '53', '47', '130', '127'],'data.csv'))

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
