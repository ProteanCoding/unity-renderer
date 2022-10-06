import * as path from 'path'
import { glob } from 'glob'
import {
  camelToSnakeCase,
  cleanGeneratedCode,
  execute,
  normalizePath,
  protocolPath,
  protocPath,
  workingDirectory,
} from './helpers'
import * as fs from 'node:fs'
import * as fse from 'fs-extra'

const componentsRawInputPath = normalizePath(
  path.resolve(protocolPath),
)

const componentsPreProccessInputPath = normalizePath(
  path.resolve(__dirname, '../temp-components/'),
)

const componentsOutputPath = path.resolve(
  __dirname,
  '../../unity-renderer/Assets/DCLPlugins/ECS7/ProtocolBuffers/Generated/',
)

async function main() {
  if (fs.existsSync(componentsPreProccessInputPath)) {
    fs.rmSync(componentsPreProccessInputPath, { recursive: true })
  }
  fse.copySync(componentsRawInputPath, componentsPreProccessInputPath, {
    overwrite: true,
  })

  await execute(`${protocPath} --version`, workingDirectory)

  await buildComponents()

  fs.rmSync(componentsPreProccessInputPath, { recursive: true })
}

const regex = new RegExp(/option *\(ecs_component_id\) *= *([0-9]+) *;/)

const getComponentId = (text: string): string | null => {
  const res = text.match(regex)
  if (res && res.length >= 1) return res[1]
  return null
}

type ComponentData = { componentName: string; componentId: number }

function generateComponentsEnum(components: ComponentData[]) {
  components.sort((first, second) => first.componentId - second.componentId)
  let content: string = ''

  content += '/* Autogenerated file, DO NOT EDIT! */\n'
  content += 'namespace DCL.ECS7\n'
  content += '{\n'
  content += '    public static class ComponentID \n'
  content += '    {\n'

  content += '        public const int TRANSFORM = 1;\n'
  for (const component of components) {
    let componentUpperCaseName = camelToSnakeCase(
      component.componentName,
    ).toUpperCase()

    content += `        public const int ${componentUpperCaseName} = ${component.componentId.toString()};\n`
  }
  content += '    }\n'
  content += '}\n'

  const outputPath = path.resolve(componentsOutputPath, 'ComponentID.gen.cs')
  fs.writeFileSync(outputPath, content)
}

async function preProcessComponents() {
  const protoFiles = glob.sync(
    normalizePath(path.resolve(componentsPreProccessInputPath, 'ecs/components/**/*.proto')),
  )
  const components: ComponentData[] = []

  for (const file of protoFiles) {
    const content = fs.readFileSync(file).toString()
    const lines = content.split('\n')
    const outputLines = new Array<string>()
    let newComponentId = null

    for (const line of lines) {
      const componentId = getComponentId(line)
      if (componentId) {
        newComponentId = Number(componentId)
      } else if (line.indexOf('common/id.proto') == -1) {
        outputLines.push(line)
      }
    }

    outputLines.push('package decentraland.ecs;')
    outputLines.push('option csharp_namespace = "DCL.ECSComponents";')

    if (newComponentId) {
      const fileName = path.basename(file)
      const componentName = fileName.replace('.proto', '')
      components.push({
        componentId: newComponentId,
        componentName,
      })
    }

    fs.writeFileSync(file, outputLines.join('\n'))
  }

  generateComponentsEnum(components)
}
async function buildComponents() {
  console.log('Building components...')
  cleanGeneratedCode(componentsOutputPath)
  await preProcessComponents()

  const protoFiles = glob
    .sync(
      normalizePath(path.resolve(componentsPreProccessInputPath, 'ecs/components/**/*.proto')),
    )
    .join(' ')

  let command = `${protocPath}`
  command += ` --csharp_out "${componentsOutputPath}"`
  command += ` --csharp_opt=file_extension=.gen.cs`
  command += ` --proto_path "${componentsPreProccessInputPath}/"`
  command += ` ${protoFiles}`

  console.log('command: ', componentsPreProccessInputPath)
  await execute(command, workingDirectory)

  console.log('Building components... Done!')
}

main().catch((err) => {
  console.error(err)
  process.exit(1)
})
