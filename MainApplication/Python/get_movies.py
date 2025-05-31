import csv
import ast

# Configuration
csv_file_path     = 'movies.csv'
insert_sql_file   = 'insert_movies.sql'
delete_sql_file   = 'delete_movies.sql'
table_name        = 'Movie'

# DB columns in desired order
db_columns = [
    'Title',
    'Production_Date',
    'Director',
    'Producer',
    'ScreenWriter',
    'Synopsis'
]

def escape_sql(value):
    """Escape for SQL, wrap in single-quotes or produce NULL."""
    if not value:
        return '\'\''
    v = str(value).replace("'", "''")
    return f"'{v}'"

def extract_from_crew(crew_str, job_names):
    """
    Parse the 'crew' field (Python-style list-of-dicts) 
    and return the first matching name for any job in job_names.
    """
    try:
        crew_list = ast.literal_eval(crew_str)
    except Exception:
        return None

    if not isinstance(crew_list, list):
        return None

    for job in job_names:
        for member in crew_list:
            if member.get('job') == job:
                return member.get('name')
    return None

def generate_insert_statement(row):
    # Map CSV → our columns
    title    = row.get('title', '').strip()
    prod_dt  = row.get('release_date', '').strip()
    director = row.get('director', '').strip()
    producer = extract_from_crew(row.get('crew', ''), ['Producer'])
    screen   = extract_from_crew(row.get('crew', ''), ['Screenplay', 'Screenwriter', 'Writer'])
    synopsis = row.get('overview', '').strip()

    values = [escape_sql(x) for x in [title, prod_dt, director, producer, screen, synopsis]]
    cols   = ', '.join(db_columns)
    vals   = ', '.join(values)
    return f"INSERT INTO {table_name} ({cols}) VALUES ({vals});"

def generate_delete_statement(row):
    # Same mapping as above
    title    = row.get('title', '').strip()
    prod_dt  = row.get('release_date', '').strip()
    director = row.get('director', '').strip()
    producer = extract_from_crew(row.get('crew', ''), ['Producer'])
    screen   = extract_from_crew(row.get('crew', ''), ['Screenplay', 'Screenwriter', 'Writer'])
    synopsis = row.get('overview', '').strip()

    clauses = []
    for col, val in zip(db_columns, [title, prod_dt, director, producer, screen, synopsis]):
        if not val:
            clauses.append(f"{col} IS NULL")
        else:
            clauses.append(f"{col} = {escape_sql(val)}")
    where = ' AND '.join(clauses)
    return f"DELETE FROM {table_name} WHERE {where};"

with open(csv_file_path, 'r', encoding='utf-8') as csvfile, \
     open(insert_sql_file,   'w', encoding='utf-8') as ins, \
     open(delete_sql_file,   'w', encoding='utf-8') as dele:

    reader = csv.DictReader(csvfile)
    for row in reader:
        ins.write(generate_insert_statement(row) + '\n')
        dele.write(generate_delete_statement(row) + '\n')

print("Done. Inserts ->", insert_sql_file)
print("Done. Deletes ->", delete_sql_file)
